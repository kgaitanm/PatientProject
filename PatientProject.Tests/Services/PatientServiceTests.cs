using Xunit;
using Moq;
using System;
using PatientProject.Application.Dtos;
using PatientProject.Application.Services;
using PatientProject.Domain.Aggregates.PatientAggregate;
using PatientProject.Domain.Exceptions;
using PatientProject.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace PatientProject.Tests.Services
{
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository> _mockRepository;
        private readonly Mock<IConfiguration> _configuration;
        private readonly EncryptedSocialInsuranceNumber _mockEncryptionHelper;

        private readonly PatientService _patientService;

        public PatientServiceTests()
        {
            _mockRepository = new Mock<IPatientRepository>();
            _configuration = new Mock<IConfiguration>();
            _mockEncryptionHelper = new EncryptedSocialInsuranceNumber(_configuration.Object);
            _patientService = new PatientService(_mockRepository.Object, _mockEncryptionHelper);
        }

        [Fact]
        public void UpdatePatient_ShouldThrowException_WhenPatientDtoIsNull()
        {
            Assert.Throws<DomainException>(() => _patientService.UpdatePatient(Guid.NewGuid(), null));
        }

        [Fact]
        public void UpdatePatient_ShouldThrowException_WhenPatientNotFound()
        {
            _mockRepository.Setup(repo => repo.FindById(It.IsAny<Guid>())).Returns((Patient)null);
           
            Assert.Throws<DomainException>(() => _patientService.UpdatePatient(Guid.NewGuid(), new PatientDto()));
        }

        [Fact]
        public void UpdatePatient_ShouldUpdateContactInformation_WhenPatientExists()
        {
            var patient = new Patient(Guid.NewGuid(), "Kevin", "Gaitan", DateTime.Parse("1991-12-01"), new PatientContact("managua", "kgaitanm@outlook.com", "+505 8481-8399"), new byte[5], new byte[5]);
            _mockRepository.Setup(repo => repo.FindById(It.IsAny<Guid>())).Returns(patient);

            var patientDto = new PatientDto
            {
                Address = "Masaya",
                Email = "kgaitanm@gmail.com",
                PhoneNumber = "+505 2263-3055"
            };

            _patientService.UpdatePatient(Guid.NewGuid(), patientDto);
           
            _mockRepository.Verify(repo => repo.Update(It.Is<Patient>(p =>
                p.Contact.Address == patientDto.Address &&
                p.Contact.Email == patientDto.Email &&
                p.Contact.PhoneNumber == patientDto.PhoneNumber)), Times.Once);
        }

    }
}