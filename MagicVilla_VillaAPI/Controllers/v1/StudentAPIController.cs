﻿using Microsoft.AspNetCore.Mvc;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Data;
using Microsoft.AspNetCore.JsonPatch;
using AutoMapper;
using Microsoft.AspNetCore.RateLimiting;
using Students_API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using SqlKata.Compilers;
using SqlKata.Execution;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Students_API.Controllers.v1
{
    [Route("api/v{version:apiVersion}/studentAPI")]
    [ApiController]
    [EnableRateLimiting("fixed")]
    [ApiVersion("1.0")]

    public class StudentAPIController : ControllerBase
    {
        private readonly ApplicationDBContext _db;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;
        private readonly ILogger<StudentAPIController> _logger;
        private readonly IConfiguration _configuration;
        private readonly QueryFactory _dbFromSqlkata;

        public StudentAPIController(ApplicationDBContext db, IMapper mapper, ICacheService cache, ILogger<StudentAPIController> logger, IConfiguration configuration , QueryFactory dbFromSqlkata)
        {
            _db = db;
            _mapper = mapper;
            _cache = cache;
            _logger = logger;
            _configuration = configuration;
            _dbFromSqlkata = dbFromSqlkata;
        }


        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            // getting data from cache
            //var students = _cache.GetData<IEnumerable<Student>>("students");
            //if (students != null && students.Count() > 0)
            //{
            //    return Ok(new { source = "cache", apiVersion = 1, data = students });
            //}

            var studentsFromDB = await _db.Students.ToListAsync();

            // saving data to cache if not exists
            var expiryTime = DateTimeOffset.Now.AddMinutes(2);
            _cache.SetData<IEnumerable<Student>>("students", studentsFromDB, expiryTime);

            // call with sql kata
            //using var connection = new SqlConnection("Server=.;Database=Students_DB_API;TrustServerCertificate=True;Trusted_Connection=True;MultipleActiveResultSets=true");

            //using var connection = new SqlConnection(_configuration.GetConnectionString("defaultSQlConnection"));
            //var compiler = new SqlServerCompiler();
            //var db = new QueryFactory(connection, compiler);

            //var studentsFromSqlKata = _dbFromSqlkata.Query("students").Get<Student>().ToList();

            //{ "TESTBEDID":1.0,"NAME":"INGURWN130117","ENVID":1.0,"HOSTNAME":null,"LASTCONNECTDATE":null,"ACTIVEFLAG":0.0,"SERIAL_NUMBER":null,"HOST_ID":null,"AFLAG":null,"INSTANCE_NUMBER":0.0}


            _dbFromSqlkata.Query("TESTBED").Insert(new { TESTBEDID=2.0, NAME="test", ENVID=1.0, HOSTNAME="test", ACTIVEFLAG=0.0, INSTANCE_NUMBER=1.0 });

            var testbeds = _dbFromSqlkata.Query("TESTBED").Get();

            foreach (var testbed in testbeds)
            {
                var json = JsonConvert.SerializeObject(testbed);
                await Console.Out.WriteLineAsync(json);
            }


            return Ok(new { source = "database", apiVersion = 1 , data = studentsFromDB });
        }


        [HttpGet("{id}", Name = "GetStudentById")]

        // this is much more readable 
        [ProducesResponseType(StatusCodes.Status200OK)] // possible return status from this request
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        //[ProducesResponseType(200)] // possible return status from this request
        //[ProducesResponseType(200 , Type = typeof(StudentDTO)] // possible return status from this request
        //[ProducesResponseType(404)]
        //[ProducesResponseType(400)]
        public ActionResult<StudentDTO> GetStudentById(int id)
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
        [HttpDelete("{id}", Name = "RemoveStudentv1")]
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

        [HttpPut("{id}", Name = "UpdateStudentv1")]
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

        [HttpPatch("{id}", Name = "PartialUpdateStudentv1")]
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


        // api health check endpoint
        [HttpGet("apihealth")]
        public IActionResult GetApiHeatlh() {
            return Ok(new {status = "healthy"});
        }

    }
}
