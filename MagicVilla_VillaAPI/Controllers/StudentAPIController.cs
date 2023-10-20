using Microsoft.AspNetCore.Mvc;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Data;
using Microsoft.AspNetCore.JsonPatch;
using AutoMapper;
using Microsoft.AspNetCore.RateLimiting;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/studentAPI")]
    [ApiController]
    [EnableRateLimiting("fixed")]
    
    public class StudentAPIController : ControllerBase
    {
        private readonly ApplicationDBContext _db;
        private readonly IMapper _mapper;

        public StudentAPIController(ApplicationDBContext db , IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }



        public ActionResult<StudentDTO> CreatedAT { get; private set; }
        public ApplicationDBContext Db { get; }

        [HttpGet]
        public IActionResult GetStudents()
        {
            return Ok(_db.Students.ToList());
        }


        [HttpGet("id", Name = "GetStudent")]

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

            var student = _db.Students.FirstOrDefault(u => u.Id == id);

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

            //student.Id = StudentData.StudentsList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            //StudentData.StudentsList.Add(student);

            Student model = _mapper.Map<Student>(student);

            _db.Students.Add(model);
            _db.SaveChanges();

            return CreatedAtRoute("GetStudent", new { id = student.Id }, model);
        }



        // delete request
        [HttpDelete("id", Name = "RemoveStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult RemoveStudent(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            //var student = StudentData.StudentsList.FirstOrDefault(u => u.Id == id);
            var student = _db.Students.FirstOrDefault(u => u.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            _db.Students.Remove(student);
            _db.SaveChanges();

            return NoContent();
        }

        [HttpPut("id", Name = "UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public IActionResult UpdateStudent(int id, [FromBody] StudentDTO updatedStudent)
        {
            //if (id != updatedStudent.Id)
            //{
            //    return BadRequest();
            //}

            Student model = _mapper.Map<Student>(updatedStudent);
            model.Id = id;
            _db.Students.Update(model);
            _db.SaveChanges();

            return NoContent();
        }

        [HttpPatch("id" , Name = "PartialUpdateStudent")]
        public IActionResult PartialUpdateStudent(int id , JsonPatchDocument<StudentDTO> patchStudentDTO)
        {
            if(patchStudentDTO == null || id == 0)
            {
                return BadRequest();
            }

            var student = _db.Students.FirstOrDefault(u => u.Id == id);

            if(student == null)
            {
                return NotFound();
            }

            StudentDTO studentDTO = _mapper.Map<StudentDTO>(student);


            patchStudentDTO.ApplyTo(studentDTO, ModelState);

            Student model = _mapper.Map<Student>(studentDTO);


            _db.Students.Update(model);
            _db.SaveChanges();

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return NoContent();
        }

    }
}
