using Microsoft.AspNetCore.Mvc;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Data;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.JsonPatch;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/studentAPI")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {
        public ActionResult<StudentDTO> CreatedAT { get; private set; }

        [HttpGet]
        public IEnumerable<StudentDTO> GetStudents()
        {
            return StudentData.StudentsList;

        }


        [HttpGet("id:int", Name = "GetStudent")]

        // this is much more readable 
        [ProducesResponseType(StatusCodes.Status200OK)] // possible return status from this request
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        //[ProducesResponseType(200)] // possible return status from this request
        //[ProducesResponseType(200 , Type = typeof(StudentDTO)] // possible return status from this request
        //[ProducesResponseType(404)]
        //[ProducesResponseType(400)]
        public ActionResult<StudentDTO> GetStudent(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var student = StudentData.StudentsList.FirstOrDefault(u => u.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<StudentDTO> AddStudent([FromBody] StudentDTO student)
        {
            if (student == null)
            {
                return BadRequest();
            }

            student.Id = StudentData.StudentsList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            StudentData.StudentsList.Add(student);

            //Console.WriteLine(StudentData.StudentsList);

            //return Ok(student);
            return CreatedAtRoute("GetStudent", new { id = student.Id }, student);
        }



        // delete request
        [HttpDelete("id:int", Name = "RemoveStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult RemoveStudent(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var student = StudentData.StudentsList.FirstOrDefault(u => u.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            StudentData.StudentsList.Remove(student);
            return NoContent();
        }

        [HttpPut("id:int", Name = "UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public IActionResult UpdateStudent(int id, [FromBody] StudentDTO updatedStudent)
        {
            if (id != updatedStudent.Id)
            {
                return BadRequest();
            }

            var student = StudentData.StudentsList.FirstOrDefault(u => u.Id == id);
            student.Name = updatedStudent.Name;

            return NoContent();
        }

        [HttpPatch("id:int" , Name = "PartialUpdateStudent")]

        public IActionResult PartialUpdateStudent(int id , JsonPatchDocument<StudentDTO> patchStudentDTO)
        {
            if(patchStudentDTO == null || id == 0)
            {
                return BadRequest();
            }

            var student = StudentData.StudentsList.FirstOrDefault(u => u.Id == id);
            if(student == null)
            {
                return NotFound();
            }

            patchStudentDTO.ApplyTo(student, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return NoContent(); 


        }




    }
}
