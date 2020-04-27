using Kollokwium_1.DTOs.Responde;
using Kollokwium_1.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kollokwium_1.Controllers
{
    [Route("api/medicaments")]
    [ApiController]
    public class JournalPrescriptionController : Controller
    {
        private readonly IHospitalDb _hospitalDbService;

        public JournalPrescriptionController(IHospitalDb hospitalDbService) {
            _hospitalDbService = hospitalDbService;
        }


        
         [HttpGet("{id}")]
        public IActionResult getPrescription(int id) 
        {
            ICollection<PrescriptionResponde> respondeList = _hospitalDbService.getPrescriptions(id);
            if (respondeList == null) return BadRequest("Podaleś źly parametr");
            return Ok(respondeList);
        }


        [Route("api/patients")]
        [HttpDelete("{id}")]
         
        public IActionResult deletePatientAndInfo(int id) {
            int value = _hospitalDbService.deletePatientAndInfo(id);
            if(value == 200) return Ok("Patient was deleted");
            return NotFound("Patient wasn't deleted");
        }
    }
}
