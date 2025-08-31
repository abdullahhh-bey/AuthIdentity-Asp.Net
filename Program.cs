using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Text;
using UserAuthManagement.Data;
using UserAuthManagement.Modals;
using UserAuthManagement.Repository;
using UserAuthManagement.Roles;
using UserAuthManagement.Services;
using UserAuthManagement.Services.ErrorController;
using YourApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// 1) EF Core
builder.Services.AddDbContext<UserAuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));

// 2) Identity (no UI)
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    // Password rules
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;

    // User
    options.User.RequireUniqueEmail = true;

    // Lockout (brute-force protection)
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.AllowedForNewUsers = true;

    // Email confirmation (turn true in production)
    options.SignIn.RequireConfirmedEmail = true;
})
.AddEntityFrameworkStores<UserAuthDbContext>()
.AddDefaultTokenProviders(); // it is necessary to generate tokens for reset pass and email confirmation



//ADDING THE JWT SETTING
var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
})
.AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
        )
    };
});


//Added the JwtService for creating tokens in this so that controllers can use this to 
//create the tokens
builder.Services.AddScoped<JwtService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddAuthorization();

builder.Services.AddScoped<StudentService, StudentService>();
builder.Services.AddScoped<TeacherService, TeacherService>();
builder.Services.AddScoped<IUnitofWork, UnitOfWork>();

builder.Services.AddExceptionHandler<GlobalErrorHandling>();
builder.Services.AddProblemDetails(); 



var app = builder.Build();


// Program.cs
// Program.cs (after var app = builder.Build(); and before app.Run();)
await SeedRolesAsync(app);

//Hello
static async Task SeedRolesAsync(WebApplication app)
{ 
    using var scope = app.Services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    foreach (var role in Role.userRoles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseExceptionHandler(); 
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
