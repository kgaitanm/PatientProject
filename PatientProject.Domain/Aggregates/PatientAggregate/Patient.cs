using PatientProject.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientProject.Domain.Aggregates.PatientAggregate
{
    public class Patient
    {
        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public PatientContact Contact { get; private set; }
        public byte[] SocialInsuranceNumber { get; private set; }
        public byte[] EncryptionIV { get; private set; }

        public int Age
        {
            get
            {
                DateTime today = DateTime.Today;
                int age = today.Year - DateOfBirth.Year;
                if (DateOfBirth > today.AddYears(-age)) age--;
                return age;
            }
        }

        public Patient(Guid id, string firstName, string lastName, DateTime dateOfBirth, PatientContact contact, byte[] encryptedSIN, byte[] iv)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Contact = contact;
            SocialInsuranceNumber = encryptedSIN;
            EncryptionIV = iv;
        }

        public void UpdatePersonalInfo(string firstName, string lastName, DateTime dateOfBirth)
        {
            FirstName = firstName ?? FirstName;
            LastName = lastName ?? LastName;
            DateOfBirth = dateOfBirth == DateTime.MinValue ? DateOfBirth : dateOfBirth;
        }

        public void SetSocialInsuranceNumber(EncryptedSocialInsuranceNumber encryptionHelper, string socialInsuranceNumber)
        {
            var (encryptedSIN, iv) = encryptionHelper.Encrypt(socialInsuranceNumber);
            SocialInsuranceNumber = encryptedSIN;
            EncryptionIV = iv;
        }

        public string GetDecryptedSocialInsuranceNumber(EncryptedSocialInsuranceNumber encryptionHelper)
        {
            return encryptionHelper.Decrypt(SocialInsuranceNumber, EncryptionIV);
        }

        public void UpdateContact(PatientContact contact)
        {
            Contact = contact;
        }
    }

}
