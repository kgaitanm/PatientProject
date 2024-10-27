using PatientProject.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientProject.Application.Interfaces
{
    public interface IPatientService
    {
        void RegisterPatient(PatientDto patientDto);
        PatientDto GetPatient(Guid id);
        void UpdatePatient(Guid id, PatientDto patientDto);
        void DeletePatient(Guid id);
        IEnumerable<PatientDto> GetAllPatients();
        Task<IEnumerable<PatientDto>> GetPatientsAboveAgeAsync(int age);
    }
}
