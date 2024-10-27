using PatientProject.Domain.Aggregates.PatientAggregate;
using PatientProject.Domain.Exceptions;
using PatientProject.Domain.Interfaces;
using PatientProject.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientProject.Application.Interfaces;
using PatientProject.Domain.Specifications;


namespace PatientProject.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly EncryptedSocialInsuranceNumber _encryptedSIN;

        public PatientService(IPatientRepository patientRepository, EncryptedSocialInsuranceNumber encryptedSIN)
        {
            _patientRepository = patientRepository;
            _encryptedSIN= encryptedSIN;
        }

        public void RegisterPatient(PatientDto patientDto)
        {
            if (patientDto == null)
                throw new DomainException("Patient data cannot be null.");

            patientDto.Id = Guid.NewGuid();
            
            var (encryptedSIN, iv) = _encryptedSIN.Encrypt(patientDto.SocialInsuranceNumber);
            
            var patient = new Patient(
                Guid.NewGuid(),
                patientDto.FirstName,
                patientDto.LastName,
                patientDto.DateOfBirth,
                new PatientContact(patientDto.Address, patientDto.Email, patientDto.PhoneNumber),
                encryptedSIN,
                iv
            );

            _patientRepository.Add(patient);
        }

        public PatientDto GetPatient(Guid id)
        {
            var patient = _patientRepository.FindById(id);
            if (patient == null)
                throw new DomainException("Patient not found.");

            return new PatientDto
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                Address = patient.Contact.Address,
                Email = patient.Contact.Email,
                PhoneNumber = patient.Contact.PhoneNumber,
                SocialInsuranceNumber = patient.GetDecryptedSocialInsuranceNumber(_encryptedSIN)
            };
        }

        public void UpdatePatient(Guid id, PatientDto patientDto)
        {
           
            if (patientDto == null)
                throw new DomainException("Patient data cannot be null.");

         
            var existingPatient = _patientRepository.FindById(id);
            if (existingPatient == null)
                throw new DomainException("Patient not found.");

          
            if (!string.IsNullOrWhiteSpace(patientDto.SocialInsuranceNumber))
            {
                existingPatient.SetSocialInsuranceNumber(_encryptedSIN, patientDto.SocialInsuranceNumber);
            }
            existingPatient.UpdatePersonalInfo(patientDto.FirstName, patientDto.LastName, patientDto.DateOfBirth);
            existingPatient.UpdateContact(new PatientContact(patientDto.Address, patientDto.Email, patientDto.PhoneNumber));

            _patientRepository.Update(existingPatient);
        }

        public void DeletePatient(Guid id)
        {
          
            var existingPatient = _patientRepository.FindById(id);
            if (existingPatient == null)
                throw new DomainException("Patient not found.");

          
            _patientRepository.Delete(id);
        }

        public async Task<IEnumerable<PatientDto>> GetPatientsAboveAgeAsync(int age)
        {
            var specification = new PatientAboveAgeSpecification(age);
            var patients = await _patientRepository.GetPatientsBySpecificationAsync(specification);

            return patients.Select(patient => new PatientDto
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                Address = patient.Contact.Address,
                Email = patient.Contact.Email,
                PhoneNumber = patient.Contact.PhoneNumber,
                SocialInsuranceNumber = _encryptedSIN.Decrypt(patient.SocialInsuranceNumber, patient.EncryptionIV)
            });
        }

        public IEnumerable<PatientDto> GetAllPatients()
        {
            var patients = _patientRepository.GetAll();
            return patients.Select(patient => new PatientDto
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                Address = patient.Contact.Address,
                Email = patient.Contact.Email,
                PhoneNumber = patient.Contact.PhoneNumber,
                SocialInsuranceNumber = _encryptedSIN.Decrypt(patient.SocialInsuranceNumber,patient.EncryptionIV)
            });
        }
    }
}
