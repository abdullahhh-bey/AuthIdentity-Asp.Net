using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS;
using Microsoft.Data.SqlClient;
using System.Security.Claims;
using UserAuthManagement.DTO;
using UserAuthManagement.Modals;
using UserAuthManagement.Roles;
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

        public AuthController(UserManager<User> usermanager, JwtService jwtservice, RoleManager<IdentityRole> rolemanager, IMapper mapper)
        {
            _usermanager = usermanager;
            _jwtservice = jwtservice;
            _rolemanager = rolemanager;
            _mapper = mapper;
        }



        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterDTO dto)
        {
            if (!Role.userRoles.Contains(dto.Role))
            {
                throw new ArgumentException();
            }

            var user = _mapper.Map<User>(dto);

            var result = await _usermanager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                throw new InvalidOperationException();
            }


            await _usermanager.AddToRoleAsync(user, dto.Role);
            return Ok("Registered Successfully");
        }


        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginDTO dto)
        {
            var user = await _usermanager.FindByEmailAsync(dto.Email);
            if (user == null)
                throw new KeyNotFoundException();

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





    }
}
