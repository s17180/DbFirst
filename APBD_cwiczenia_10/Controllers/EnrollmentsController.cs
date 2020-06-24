using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APBD_cwiczenia_10.Models;
using APBD_cwiczenia_10.Requests;
using APBD_cwiczenia_10.Responses;
using APBD_cwiczenia_10.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APBD_cwiczenia_10.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IDbService _service;
        public EnrollmentsController(IDbService service)
        {
            _service = service;
        }


        [HttpPost]      
        public IActionResult EnrollStudent(EnrollStudentRequest request) //nowy student
        {          
            try
            {
                return Ok(_service.EnrollStudent(request));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("{promotions}")]      
        public IActionResult PromoteStudents(PromotionStudentRequest request, string promotions)
        {
            if (promotions.Equals("promotions"))
            {
                try
                {

                    return Ok(_service.PromoteStudents(request.Semester, request.Studies));


                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

            }
            else
                return BadRequest("Bad request!");

        }


    }
}