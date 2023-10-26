using Microsoft.AspNetCore.Mvc;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Data;
using Microsoft.AspNetCore.JsonPatch;
using AutoMapper;
using Microsoft.AspNetCore.RateLimiting;
using Students_API.Services;
using Microsoft.EntityFrameworkCore;

namespace Students_API.Controllers.v2
{
    [Route("api/v{version:apiVersion}/studentAPI")]
    [ApiController]
    [EnableRateLimiting("fixed")]
    [ApiVersion("2.0")]
    [ApiVersion("2.1")]

    public class StudentAPIController : ControllerBase
    {
        private readonly ApplicationDBContext _db;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;
        private readonly ILogger<StudentAPIController> _logger;

        public StudentAPIController(ApplicationDBContext db, IMapper mapper, ICacheService cache, ILogger<StudentAPIController> logger)
        {
            _db = db;
            _mapper = mapper;
            _cache = cache;
            _logger = logger;
        }



        [HttpGet]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> GetStudents()
        {
            // getting data from cache
            var students = _cache.GetData<IEnumerable<Student>>("students");
            if (students != null && students.Count() > 0)
            {
                return Ok(new { source = "cache", apiVersion = 2 , data = students });
            }

            var studentsFromDB = await _db.Students.ToListAsync();

            // saving data to cache if not exists
            var expiryTime = DateTimeOffset.Now.AddMinutes(2);
            _cache.SetData<IEnumerable<Student>>("students", studentsFromDB, expiryTime);

            return Ok(new { source = "database", apiVersion = 2 , data = studentsFromDB });
        }
        

        [HttpGet]
        [MapToApiVersion("2.1")]
        public async Task<IActionResult> GetStudentsv21()
        {
            // getting data from cache
            var students = _cache.GetData<IEnumerable<Student>>("students");
            if (students != null && students.Count() > 0)
            {
                return Ok(new { source = "cache", apiVersion = 2.1 , data = students });
            }

            var studentsFromDB = await _db.Students.ToListAsync();

            // saving data to cache if not exists
            var expiryTime = DateTimeOffset.Now.AddMinutes(2);
            _cache.SetData<IEnumerable<Student>>("students", studentsFromDB, expiryTime);

            return Ok(new { source = "database", apiVersion = 2.1 , data = studentsFromDB });
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

            // getting data from cache
            var allStudents = _cache.GetData<IEnumerable<Student>>("students");
            var student = allStudents?.FirstOrDefault(u => u.Id == id);
            if (student != null)
            {
                return Ok(new { source = "cache", data = student });
            }

            var allStudentsFromDb = _db.Students.ToList();
            var studentFromDb = allStudentsFromDb?.FirstOrDefault(u => u.Id == id);

            // syncing db and cache
            var expiryTime = DateTimeOffset.Now.AddMinutes(2);
            _cache.SetData<IEnumerable<Student>>("students", allStudentsFromDb, expiryTime);

            if (studentFromDb == null)
            {
                return NotFound();
            }

            return Ok(new { source = "database", data = studentFromDb });
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

            // making cache empty
            _cache.RemoveData("students");

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

            var student = _db.Students.FirstOrDefault(u => u.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            // making changes in cache
            var allStudents = _cache.GetData<IEnumerable<Student>>("students");

            if (allStudents != null)
            {
                // if cache contains some data alredy then delete a student with specific id otherwise not
                var remainingStudents = allStudents.Where(student => student.Id != id);
                var expiryTime = DateTimeOffset.Now.AddMinutes(2);
                _cache.SetData("students", remainingStudents, expiryTime);
            }

            // making changes in database
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


            Student model = _mapper.Map<Student>(updatedStudent);
            model.Id = id;

            // making changes in cache
            var allStudents = _cache.GetData<IEnumerable<Student>>("students");

            if (allStudents != null)
            {
                var remainingStudents = allStudents.Where(student => student.Id != id).ToList();
                remainingStudents.Add(model);

                remainingStudents = remainingStudents.OrderBy(student => student.Id).ToList();

                var expiryTime = DateTimeOffset.Now.AddMinutes(2);
                _cache.SetData<IEnumerable<Student>>("students", remainingStudents, expiryTime);
            }

            _db.Students.Update(model);
            _db.SaveChanges();

            return NoContent();
        }

        [HttpPatch("id", Name = "PartialUpdateStudent")]
        public IActionResult PartialUpdateStudent(int id, JsonPatchDocument<StudentDTO> patchStudentDTO)
        {
            if (patchStudentDTO == null || id == 0)
            {
                return BadRequest();
            }

            var student = _db.Students.FirstOrDefault(u => u.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            StudentDTO studentDTO = _mapper.Map<StudentDTO>(student);


            patchStudentDTO.ApplyTo(studentDTO, ModelState);

            Student model = _mapper.Map<Student>(studentDTO);

            // making changes in cache
            var allStudents = _cache.GetData<IEnumerable<Student>>("students");

            if (allStudents != null)
            {
                var remainingStudents = allStudents.Where(student => student.Id != id).ToList();
                remainingStudents.Add(model);

                remainingStudents = remainingStudents.OrderBy(student => student.Id).ToList();

                var expiryTime = DateTimeOffset.Now.AddMinutes(2);
                _cache.SetData<IEnumerable<Student>>("students", remainingStudents, expiryTime);
            }


            _db.Students.Update(model);
            _db.SaveChanges();

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return NoContent();
        }

        // endpoint for cleaing cache
        [HttpGet("clearCache")]
        public IActionResult ClearCache()
        {
            _cache.RemoveData("students");
            return NoContent();

        }

    }
}
