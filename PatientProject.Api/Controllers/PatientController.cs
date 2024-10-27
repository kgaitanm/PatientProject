using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientProject.Api.Filters;
using PatientProject.Application.Dtos;
using PatientProject.Application.Interfaces;

namespace PatientProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilter))]
        public IActionResult CreatePatient([FromBody]PatientDto patientDto)
        {
            _patientService.RegisterPatient(patientDto);
            return CreatedAtAction(nameof(GetPatientById), new { id = patientDto.Id }, patientDto);
        }


        [HttpGet]
        public IActionResult GetPatients()
        {
            var allPatients = _patientService.GetAllPatients();
            return Ok(allPatients);
        }

        [HttpGet("{id}")]
        public IActionResult GetPatientById(Guid id)
        {
            var patientDto = _patientService.GetPatient(id);
            return Ok(patientDto);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilter))]
        public IActionResult UpdatePatient(Guid id, [FromBody] PatientDto patientDto)
        {
            _patientService.UpdatePatient(id, patientDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePatient(Guid id)
        {
            _patientService.DeletePatient(id);
            return NoContent();
        }
    }
}
