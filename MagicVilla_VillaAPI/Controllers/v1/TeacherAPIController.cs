using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Students_API.Models;
using Students_API.Data;
using Students_API.Models;
using Students_API.Models.Dto;
using Students_API.Services;

namespace Students_API.Controllers.v1
{
    [Route("api/v{version:apiVersion}/teacherAPI")]
    [ApiController]
    [ApiVersion("1.0")]
    public class TeacherAPIController : ControllerBase
    {
        private readonly ApplicationDBContext _db;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;

        public TeacherAPIController(ApplicationDBContext db, IMapper mapper, ICacheService cache)
        {
            _db = db;
            _mapper = mapper;
            _cache = cache;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTeachers()
        {
            var teachers = _cache.GetData<IEnumerable<Teacher>>("teachers");
            if (teachers != null && teachers.Count() > 0)
            {
                return Ok(new { source = "cache", data = teachers });
            }

            var teachersFromDB = await _db.Teachers.ToListAsync();

            // saving data to cache if not exists
            var expiryTime = DateTimeOffset.Now.AddMinutes(2);
            _cache.SetData<IEnumerable<Teacher>>("teachers", teachersFromDB, expiryTime);

            return Ok(new { source = "database", data = teachersFromDB });
        }

        [HttpGet("id")]
        public IActionResult GetTeacherById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            // getting data from cache
            var allTeachers = _cache.GetData<IEnumerable<Teacher>>("teachers");
            var teacher = allTeachers?.FirstOrDefault(u => u.Id == id);
            if (teacher != null)
            {
                return Ok(new { source = "cache", data = teacher });
            }

            var allTeachersFromDb = _db.Teachers.ToList();
            var teacherFromDb = allTeachersFromDb?.FirstOrDefault(u => u.Id == id);

            // syncing db and cache
            var expiryTime = DateTimeOffset.Now.AddMinutes(2);
            _cache.SetData<IEnumerable<Teacher>>("students", allTeachersFromDb, expiryTime);

            if (teacherFromDb == null)
            {
                return NotFound();
            }

            return Ok(new { source = "database", data = _mapper.Map<TeacherDto>(teacherFromDb) });

        }

        [HttpPost]
        public IActionResult AddTeacher([FromBody] TeacherDto teacher)
        {
            // making cache empty
            _cache.RemoveData("teachers");

            Teacher model = _mapper.Map<Teacher>(teacher);
            model.HiringDate = DateTime.Now;
            model.Rating = 3;

            _db.Teachers.Add(model);
            _db.SaveChanges();

            return Ok(model);
        }


        [HttpDelete("id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult RemoveTeacher(int id)
        {
            if (id == 0) return BadRequest();

            var teacher = _db.Teachers.FirstOrDefault(u => u.Id == id);

            if (teacher == null) return NotFound();

            // making changes in cache
            var allTeachers = _cache.GetData<IEnumerable<Teacher>>("teachers");

            if (allTeachers != null)
            {
                // if cache contains some data alredy then delete a student with specific id otherwise not
                var remainingTeachers = allTeachers.Where(teacher => teacher.Id != id);
                var expiryTime = DateTimeOffset.Now.AddMinutes(2);
                _cache.SetData("teachers", remainingTeachers, expiryTime);
            }


            _db.Remove(teacher);
            _db.SaveChanges();

            return NoContent();
        }

        [HttpPut("id")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult ChangeTeacherDetails(int id, [FromBody] TeacherDto newTeacher)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            Teacher oldTeacher = _db.Teachers.FirstOrDefault(u => u.Id == id);

            Teacher model = _mapper.Map<Teacher>(newTeacher);
            model.Id = id;
            model.Rating = oldTeacher.Rating;
            model.HiringDate = oldTeacher.HiringDate;

            // making changes in cache
            var allTeachers = _cache.GetData<IEnumerable<Teacher>>("teachers");

            if (allTeachers != null)
            {
                var remainingTeachers = allTeachers.Where(teacher => teacher.Id != id).ToList();
                remainingTeachers.Add(model);

                remainingTeachers = remainingTeachers.OrderBy(teacher => teacher.Id).ToList();

                var expiryTime = DateTimeOffset.Now.AddMinutes(2);
                _cache.SetData<IEnumerable<Teacher>>("teachers", remainingTeachers, expiryTime);
            }

            _db.Update(model);
            _db.SaveChanges();

            return NoContent();

        }


        [HttpPatch("id")]

        public IActionResult UpdateTeacherDetails(int id, JsonPatchDocument<TeacherDto> patchDto)
        {
            if (patchDto == null)
            {
                return BadRequest();
            }

            var teacher = _db.Teachers.FirstOrDefault(u => u.Id == id);

            if (teacher == null) return NotFound();

            TeacherDto teacherDto = _mapper.Map<TeacherDto>(teacher);

            patchDto.ApplyTo(teacherDto, ModelState);

            var newTeacher = _mapper.Map<Teacher>(teacherDto);
            newTeacher.Rating = teacher.Rating;
            newTeacher.HiringDate = teacher.HiringDate;

            // making changes in cache
            var allTeachers = _cache.GetData<IEnumerable<Teacher>>("teachers");

            if (allTeachers != null)
            {
                var remainingTeachers = allTeachers.Where(teacher => teacher.Id != id).ToList();
                remainingTeachers.Add(newTeacher);

                remainingTeachers = remainingTeachers.OrderBy(teacher => teacher.Id).ToList();

                var expiryTime = DateTimeOffset.Now.AddMinutes(2);
                _cache.SetData<IEnumerable<Teacher>>("teachers", remainingTeachers, expiryTime);
            }


            _db.Update(newTeacher);
            _db.SaveChanges();
            return NoContent();

        }

    }
}
