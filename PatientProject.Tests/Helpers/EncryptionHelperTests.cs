
using Microsoft.Extensions.Configuration;
using Moq;
using PatientProject.Application.Services;
using PatientProject.Domain.Aggregates.PatientAggregate;
using PatientProject.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientProject.Tests.Helpers
{
    public class EncryptionHelperTests
    {
        private readonly Mock<IConfiguration> _configuration;

        public EncryptionHelperTests()
        {
            _configuration = new Mock<IConfiguration>();
        }

        [Fact]
        public void Encrypt_ShouldReturnEncryptedData()
        {
           
            var helper = new EncryptedSocialInsuranceNumber(_configuration.Object);
            var plainText = "SensitiveData";

            
            var (encryptedData, iv) = helper.Encrypt(plainText);

           
            Assert.NotNull(encryptedData);
            Assert.NotNull(iv);
            Assert.NotEqual(plainText, Encoding.UTF8.GetString(encryptedData));
        }

        [Fact]
        public void Decrypt_ShouldReturnOriginalData()
        {
            
            var helper = new EncryptedSocialInsuranceNumber(_configuration.Object);
            var plainText = "SensitiveData";

           
            var (encryptedData, iv) = helper.Encrypt(plainText);
            var decryptedText = helper.Decrypt(encryptedData, iv);

           
            Assert.Equal(plainText, decryptedText);
        }
    }
}