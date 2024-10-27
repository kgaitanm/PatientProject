using PatientProject.Domain.Aggregates.PatientAggregate;
using PatientProject.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientProject.Domain.Interfaces
{
    public interface IPatientRepository
    {
        void Add(Patient patient);
        Patient FindById(Guid id);
        void Update(Patient patient);
        void Delete(Guid id);
        IEnumerable<Patient> GetAll();
        Task<IEnumerable<Patient>> GetPatientsBySpecificationAsync(ISpecification<Patient> specification);
    }
}
