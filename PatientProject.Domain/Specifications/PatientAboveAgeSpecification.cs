using PatientProject.Domain.Aggregates.PatientAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PatientProject.Domain.Specifications
{
    public class PatientAboveAgeSpecification : ISpecification<Patient>
    {
        private readonly int _age;

        public PatientAboveAgeSpecification(int age)
        {
            _age = age;
        }

        public Expression<Func<Patient, bool>> Criteria => patient => patient.Age > _age;
    }
}
