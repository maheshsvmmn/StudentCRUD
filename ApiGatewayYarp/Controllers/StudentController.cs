using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiGatewayYarp.Controllers
{
    [Route("/student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        public StudentController()
        {
            Console.WriteLine("I am inside student controller constructor.....");
        }

        [HttpGet]
        public IActionResult Get()
        {
            Console.WriteLine("I am inside get method of student controller.......");
            return Ok();
        }
    }
}
