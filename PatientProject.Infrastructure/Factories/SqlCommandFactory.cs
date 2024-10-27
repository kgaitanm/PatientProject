using System;
using Microsoft.Data.SqlClient;
using PatientProject.Domain.Aggregates.PatientAggregate;
using PatientProject.Domain.Specifications;

namespace PatientProject.Infrastructure.Factories
{
    public static class SqlCommandFactory
    {
        public static SqlCommand CreateInsertPatientCommand(Patient patient, SqlConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Patient (Id, FirstName, LastName, DateOfBirth, Address, Email, PhoneNumber, SocialInsuranceNumber, EncryptionIV) VALUES (@Id, @FirstName, @LastName, @DateOfBirth, @Address, @Email, @PhoneNumber, @SocialInsuranceNumber, @EncryptionIV)";
            command.Parameters.AddWithValue("@Id", patient.Id);
            command.Parameters.AddWithValue("@FirstName", patient.FirstName);
            command.Parameters.AddWithValue("@LastName", patient.LastName);
            command.Parameters.AddWithValue("@DateOfBirth", patient.DateOfBirth);
            command.Parameters.AddWithValue("@Address", patient.Contact.Address);
            command.Parameters.AddWithValue("@Email", patient.Contact.Email);
            command.Parameters.AddWithValue("@PhoneNumber", patient.Contact.PhoneNumber);
            command.Parameters.AddWithValue("@SocialInsuranceNumber", patient.SocialInsuranceNumber);
            command.Parameters.AddWithValue("@EncryptionIV", patient.EncryptionIV);
            return command;
        }

        public static SqlCommand CreateGetPatientByIdCommand(Guid id, SqlConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, FirstName, LastName, DateOfBirth, Address, Email, PhoneNumber, SocialInsuranceNumber, EncryptionIV FROM Patient WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", id);
            return command;
        }

        public static SqlCommand CreateGetAllPatientsCommand(SqlConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, FirstName, LastName, DateOfBirth, Address, Email, PhoneNumber, SocialInsuranceNumber, EncryptionIV FROM Patient";
            return command;
        }

        public static SqlCommand CreateUpdatePatientCommand(Patient patient, SqlConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandText = "UPDATE Patient SET FirstName = @FirstName, LastName = @LastName, DateOfBirth = @DateOfBirth, Address = @Address, Email = @Email, PhoneNumber = @PhoneNumber, SocialInsuranceNumber = @SocialInsuranceNumber, EncryptionIV = @EncryptionIV WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", patient.Id);
            command.Parameters.AddWithValue("@FirstName", patient.FirstName);
            command.Parameters.AddWithValue("@LastName", patient.LastName);
            command.Parameters.AddWithValue("@DateOfBirth", patient.DateOfBirth);
            command.Parameters.AddWithValue("@Address", patient.Contact.Address);
            command.Parameters.AddWithValue("@Email", patient.Contact.Email);
            command.Parameters.AddWithValue("@PhoneNumber", patient.Contact.PhoneNumber);
            command.Parameters.AddWithValue("@SocialInsuranceNumber", patient.SocialInsuranceNumber);
            command.Parameters.AddWithValue("@EncryptionIV", patient.EncryptionIV);
            return command;
        }

        public static SqlCommand CreateDeletePatientCommand(Guid id, SqlConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Patient WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", id);
            return command;
        }

        public static SqlCommand CreateGetPatientsBySpecificationCommand(ISpecification<Patient> specification, SqlConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM Patient WHERE {specification.Criteria}";
            return command;
        }
    }
}

