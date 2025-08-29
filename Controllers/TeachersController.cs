using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserAuthManagement.DTO;
using UserAuthManagement.Services;

namespace UserAuthManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly TeacherService _teacherService;

        public TeachersController(TeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet("list-teachers")]
        [Authorize(Roles = "Student, Advisor, COD, Teacher, Admin")]
        public async Task<IActionResult> GetTeachersBasicAPI()
        {
            try
            {
                var teachers = await _teacherService.GetTeachers();
                if (teachers.Count == 0)
                    return NotFound();

                return Ok(teachers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("list-teachers-details")]
        [Authorize(Roles = "COD, Admin")]
        public async Task<IActionResult> GetTeachersDetailsAPI()
        {
            try
            {
                var teachers = await _teacherService.GetTeachersAll();
                if (teachers.Count == 0)
                    return NotFound();

                return Ok(teachers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("teacher/{id}")]
        [Authorize(Roles = "Student, Advisor, COD, Teacher, Admin")]
        public async Task<IActionResult> GetTeacherByIdAPI(int id)
        {
            try
            {
                var teacher = await _teacherService.GetTeacherById(id);
                if(teacher == null)
                    return NotFound();

                return Ok(teacher);
            } catch ( Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("teacher-details/{id}")]
        [Authorize(Roles = "COD, Admin")]
        public async Task<IActionResult> GetTeacherDetailsByIdAPI(int id)
        {
            try
            {
                var teacher = await _teacherService.GetTeacherDetailsById(id);
                if (teacher == null)
                    return NotFound();

                return Ok(teacher);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("add-teacher")]
        [Authorize(Roles = "COD, Admin")]
        public async Task<IActionResult> AddTeacherAPI([FromBody] CreateTeacherDTO dto)
        {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest("Enter Complete Info");

                var check = await _teacherService.AddTeacher(dto);
                if (!check)
                    return BadRequest("Email Already Exist!");

                return Ok("Teacher Created Successfully");
            } catch ( Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("update-teacher/{id}")]
        [Authorize(Roles = "COD, Admin")]
        public async Task<IActionResult> UpdateTeacherAPI(int id,[FromBody] UpdateTeacher dto)
        {
            try
            {
                var check = await _teacherService.UpdateTeacher(id, dto);
                if(!check)
                    return BadRequest("Invalid Info\nTry a different Email");

                return Ok("Teacher Updated Successfully");
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpDelete("remove-teacher/{email}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveTeacherAPI(string email)
        {
            try
            {
                var check = await _teacherService.RemoveTeacher(email);
                if(!check)
                    return NotFound();

                return NoContent();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
