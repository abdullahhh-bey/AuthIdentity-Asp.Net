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
    public class StudentsController : ControllerBase
    {
        private readonly StudentService _studentService;

        public StudentsController(StudentService studentService)
        {
            _studentService = studentService;
        }


        [HttpGet("list-students")]
        [Authorize(Roles = "Admin, Teacher, Advisor, COD")]
        public async Task<IActionResult> GetStudentApi()
        {
            try
            {
                var students = await _studentService.GetStudents();
                return Ok(students);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Teacher, Advisor, COD")]
        public async Task<IActionResult> GetStudentByIdAPI(int id)
        {
            try
            {
                var students = await _studentService.GetStudentById(id);
                if (students == null)
                    return NotFound("Student not Exist!");

                return Ok(students);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("add-student")]
        [Authorize(Roles = "Admin, Advisor, COD")]
        public async Task<IActionResult> AddStudentAPI([FromBody] CreateStudentDTO dto)
        {
            try
            {
                var students = await _studentService.AddStudent(dto);
                if (!students)
                    return BadRequest("Email Already Exists");

                return Ok("User Created Successfully!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPut("update-student/{id}")]
        [Authorize(Roles = "Admin, Advisor, COD")]
        public async Task<IActionResult> UpdateStudentAPI(int id, [FromBody] UpdateStudentDTO dto)
        {
            try
            {
                var students = await _studentService.UpdateStudent(id ,dto);
                if (!students)
                {
                    return BadRequest("Email Already Exist!\nInvalid Credentials");
                }
                return Ok("User Updated Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("remove-student/{id}")]
        [Authorize(Roles = "Admin, COD")]
        public async Task<IActionResult> RemoveStudentAPI(int id)
        {
            try
            {
                var students = await _studentService.RemoveStudent(id);
                if (!students)
                {
                    return BadRequest("Student doesn't Exist!");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
