using AutoMapper;
using MagicVilla_VillaAPI.Data;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Students_API.Data;
using Students_API.Models;
using Students_API.Models.Dto;

namespace Students_API.Controllers
{
    [Route("api/teacherAPI")]
    [ApiController]
    public class TeacherAPIController : ControllerBase
    {
        private readonly ApplicationDBContext _db;
        private readonly IMapper _mapper;

        public TeacherAPIController(ApplicationDBContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetTeachers()
        {
            return Ok(_db.Teachers.ToList());
        }

        [HttpGet("id")]
        public IActionResult GetTeacherById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var teacher = _db.Teachers.FirstOrDefault(u => u.Id == id);

            if (teacher == null) return NotFound();

            return Ok(_mapper.Map<TeacherDto>(teacher));
        }

        [HttpPost]
        public IActionResult AddTeacher([FromBody] TeacherDto teacher)
        {
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
        public IActionResult RemoveTeacher(int id) {
            if (id == 0) return BadRequest();

            var teacher = _db.Teachers.FirstOrDefault(u => u.Id == id);

            if (teacher == null) return NotFound();

            _db.Remove(teacher);
            _db.SaveChanges();

            return NoContent();
        }

        [HttpPut("id")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult ChangeTeacherDetails(int id , [FromBody] TeacherDto newTeacher)
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

            _db.Update(model);
            _db.SaveChanges();

            return NoContent();

        }


        [HttpPatch("id")]
        
        public IActionResult UpdateTeacherDetails(int id , JsonPatchDocument<TeacherDto> patchDto)
        {
            if(patchDto == null)
            {
                return BadRequest();
            }

            var teacher = _db.Teachers.FirstOrDefault(u => u.Id==id);

            if (teacher == null) return NotFound();

            TeacherDto teacherDto = _mapper.Map<TeacherDto>(teacher);

            patchDto.ApplyTo(teacherDto, ModelState);

            var newTeacher = _mapper.Map<Teacher>(teacherDto);
            newTeacher.Rating = teacher.Rating;
            newTeacher.HiringDate = teacher.HiringDate;

            _db.Update(newTeacher);
            _db.SaveChanges();
            return NoContent();
                    
        }

    }
}
