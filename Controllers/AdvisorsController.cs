using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserAuthManagement.DTO;
using UserAuthManagement.Modals;
using UserAuthManagement.Services;

namespace UserAuthManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvisorsController : ControllerBase
    {
        private readonly AdvisorService _advisor;

        public AdvisorsController(AdvisorService advisor)
        {
            _advisor = advisor;
        }


        [HttpGet("list-advisor")]
        [Authorize(Roles = "Admin, COD, Teacher, Advisor, Student")]
        public async Task<IActionResult> GetAdvisorAPI()
        {
            var dto = await _advisor.GetAllAdvisor();
            if (dto.Count == 0)
                return Ok("No Advisor Found");
            
            return Ok(dto);
        }


        [HttpGet("list-advisor-details")]
        [Authorize(Roles = "Admin, COD")]
        public async Task<IActionResult> GetAdvisorDetailsAPI()
        {
            var dto = await _advisor.GetAllAdvisorDetails();
            if (dto.Count == 0)
                return Ok("No Advisor Found");

            return Ok(dto);
        }



        [HttpGet("advisor/{id}")]
        [Authorize(Roles = "Admin, COD, Teacher, Advisor, Student")]
        public async Task<IActionResult> GetAdvisorByIdAPI(int id)
        {
            var dto = await _advisor.GetAdvisorById(id);
            if (dto.Name == "")
                throw new KeyNotFoundException();

            return Ok(dto);
        }


        [HttpGet("advisor-details/{id}")]
        [Authorize(Roles = "Admin, COD")]
        public async Task<IActionResult> GetAdvisorDetailsByIdAPI(int id)
        {
            var dto = await _advisor.GetAdvisorDetailsById(id);
            if (dto.Name == "")
                throw new KeyNotFoundException();

            return Ok(dto);
        }


        [HttpPost("add-advisor")]
        [Authorize(Roles = "Admin , COD")]
        public async Task<IActionResult> CreateAdvisorAPI([FromBody] CreateAdvisorDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var check = await _advisor.CreateAdvisor(dto);
            if (!check)
                return BadRequest("Email Already Exists");

            return Ok("Created");
 
        }



        [HttpPut("update-advisor/{id}")]
        [Authorize(Roles = "Admin , COD")]
        public async Task<IActionResult> UpdateAdvisorAPI(int id, [FromBody] UpdateAdvisorDTO dto)
        {
            var check = await _advisor.UpdateAdvisor(id, dto);
            if (!check)
                throw new BadHttpRequestException("Error can be :\nAdvisor doesn't exist\nEmail Already Registered");

            return Ok("Advisor Updated Successfully!");
        }


        [HttpDelete("remove-advisor/{email}")]
        [Authorize(Roles = "Admin ")]
        public async Task<IActionResult> RemoveAdvisorAPI(string email)
        {
            var check = await _advisor.RemoveAdvisor(email);
            if (!check)
                throw new KeyNotFoundException();

            return NoContent();
        }


    }
}
