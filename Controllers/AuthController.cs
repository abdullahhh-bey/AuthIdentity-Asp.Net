using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Security.Claims;
using System.Text;
using UserAuthManagement.DTO;
using UserAuthManagement.Modals;
using UserAuthManagement.Roles;
using UserAuthManagement.Services.EmailService;
using YourApi.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UserAuthManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _usermanager;
        private readonly JwtService _jwtservice;
        private readonly RoleManager<IdentityRole> _rolemanager;
        private readonly EmailService _emailservice;

        public AuthController(UserManager<User> usermanager, JwtService jwtservice, RoleManager<IdentityRole> rolemanager, IMapper mapper, EmailService emailservice)
        {
            _usermanager = usermanager;
            _jwtservice = jwtservice;
            _rolemanager = rolemanager;
            _mapper = mapper;
            _emailservice = emailservice;
        }



        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterDTO dto)
        {
            if (!Role.userRoles.Contains(dto.Role))
            {
                throw new ArgumentException("Invalid Role");
            }

            var user = _mapper.Map<User>(dto);

            var result = await _usermanager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                throw new InvalidOperationException();
            }
            await _usermanager.AddToRoleAsync(user, dto.Role);


            var token = await _usermanager.GenerateEmailConfirmationTokenAsync(user);

            var subject = $"Email Confirmation";
            var message = $"Your token : {token}\nEnter your email with this token and confirm it!";

            await _emailservice.SendEmailsAsync(dto.Email, subject, message);
            return Ok("Confirmation Email has been sent to your registered email");
        }




        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginDTO dto)
        {
            var user = await _usermanager.FindByEmailAsync(dto.Email);
            if (user == null)
                throw new KeyNotFoundException();

            if (!user.EmailConfirmed)
                throw new InvalidOperationException("Email not confirmed yet!");

            var checkPass = await _usermanager.CheckPasswordAsync(user, dto.Password);
            if (!checkPass)
                throw new UnauthorizedAccessException();

            var token = await _jwtservice.GenerateToken(user);
            return Ok(token);

        }



        //Router after AUTHENTICATION
        //When you ut this Authorize, it means when accessing this action/endpoint, you must pass a token otherwise it will pass 401
        [Authorize(Roles = "Student, Advisor, Teacher, COD, Admin")]
        [HttpGet("hello")]
        public IActionResult SecureRoute()
        {
            return Ok("Hello, Valid USER!");
        }

        //Pass token  + yoou have to bee Admin otherwise it will return 403 error 
        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public IActionResult AdminRoute()
        {
            return Ok("Hello, Admin!");
        }


        //Validations

        [HttpPost("change-password")]
        [Authorize(Roles = "Student , Admin , COD , Advisor , Teacher")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // get user id from token
            var user = await _usermanager.FindByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException();

            var result = await _usermanager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!result.Succeeded)
                throw new ArgumentException();

            return Ok("Password Changed Successfully!");
        }




        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO dto)
        {
            var user = await _usermanager.FindByEmailAsync(dto.Email);
            if (user == null)
                return BadRequest("User not found");

            // Generate reset token
            var token = await _usermanager.GeneratePasswordResetTokenAsync(user);

            // Encode for URL/email safety
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            

            // Generate OTP
            var otp = new Random().Next(100000, 999999).ToString();

            // Store OTP in user claims (simple, but better use a DB table)
            await _usermanager.SetAuthenticationTokenAsync(user, "MyApp", "PasswordResetOtp", otp);

            // Build reset link
            var resetLink = $"https://yourfrontend.com/reset-password?email={user.Email}&token={encodedToken}&otp={otp}";

            await _emailservice.SendEmailsAsync(dto.Email, "Password Reset",
                $"Your OTP is: {otp}\n\nReset using this link:\n{resetLink}");

            return Ok("Password reset instructions sent to your email.");
        }



        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Incomplete information");

            var user = await _usermanager.FindByEmailAsync(dto.Email);
            if (user == null)
                return BadRequest("Invalid email");

            // Verify OTP
            var storedOtp = await _usermanager.GetAuthenticationTokenAsync(user, "MyApp", "PasswordResetOtp");
            if (storedOtp != dto.Otp)
                return BadRequest("Invalid OTP");

            // Decode token properly
            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(dto.Token));
            Console.WriteLine("Decoded Token: " + decodedToken);

            var result = await _usermanager.ResetPasswordAsync(user, decodedToken, dto.NewPassword);

            if (!result.Succeeded)
                throw new ArgumentException("Something weng wrong");

            // Remove OTP after successful reset
            await _usermanager.RemoveAuthenticationTokenAsync(user, "MyApp", "PasswordResetOtp");

            return Ok("Password has been reset successfully!");
        }







        [HttpPost("email-confirmation")]
        public async Task<IActionResult> ConfirmedEmail([FromBody] ConfirmEmailDTO dto)
        {
            if (!ModelState.IsValid)
                throw new ArgumentException("Incomplete Information");

            var user = await _usermanager.FindByEmailAsync(dto.Email);
            if (user == null)
                throw new KeyNotFoundException("Wrong Information!");

            var result = await _usermanager.ConfirmEmailAsync(user, dto.Token);
            if (!result.Succeeded)
                throw new ArgumentException("Wrong Information!");

            return Ok("Email Confirmed.\nUser registered!");
        }


    }
}
