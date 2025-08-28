using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Text;
using UserAuthManagement.Data;
using UserAuthManagement.Modals;
using UserAuthManagement.Roles;
using YourApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// 1) EF Core
builder.Services.AddDbContext<UserAuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));

// 2) Identity (no UI)
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    // Keep defaults for now (we’ll tweak password rules later if you want)
})
.AddEntityFrameworkStores<UserAuthDbContext>()
.AddDefaultTokenProviders();





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


//Afdded the JwtService dfor creating tokens in this so that controllers can use this to 
//create the tokens
builder.Services.AddScoped<JwtService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAuthorization();


var app = builder.Build();


// Program.cs
// Program.cs (after var app = builder.Build(); and before app.Run();)
await SeedRolesAsync(app);

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

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
