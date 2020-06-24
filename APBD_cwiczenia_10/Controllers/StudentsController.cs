using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APBD_cwiczenia_10.Requests;
using APBD_cwiczenia_10.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace APBD_cwiczenia_10.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {

        //private const string ConString = "Data Source=db-mssql;Initial Catalog=s16503;Integrated Security=True;";

        public IConfiguration Configuration { get; set; }

        private readonly IDbService _service;
        public StudentsController(IDbService service)
        {
            _service = service;
        }


        [HttpGet]
        public IActionResult GetStudents()
        {          
            return Ok(_service.GetStudents());
        }

     


        [HttpPut("{index}")]
        public IActionResult UpdateStudnet(UpdateStudentRequest request, string index)  // aktualizacja studenta
        {
          try
            {
            var student = _service.UpdateStudent(index, request);
            return Ok(student);
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }


            
        }

        [HttpDelete("{index}")]
        public IActionResult DeleteStudnet(string index)  // usuwanie studenta
        {

            try
            {
                _service.DeleteStudent(index);

            }catch(Exception exc)
            {
                return NotFound(exc.Message);
            }

                   
            return Ok("usunieto studenta");
        }
    }
}