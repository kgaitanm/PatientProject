using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection.Metadata;
using Microsoft.Data.SqlClient;
using PatientProject.Domain.Aggregates.PatientAggregate;
using PatientProject.Domain.Interfaces;
using PatientProject.Domain.Specifications;
using PatientProject.Infrastructure.Database;
using PatientProject.Infrastructure.Factories;

namespace PatientProject.Infrastructure.Repositories.PatientRepository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public PatientRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void Add(Patient patient)
        {

            var command = SqlCommandFactory.CreateInsertPatientCommand(patient, _dbConnection.GetConnection());

            command.ExecuteNonQuery();

        }

        public Patient FindById(Guid id)
        {

            var command = SqlCommandFactory.CreateGetPatientByIdCommand(id, _dbConnection.GetConnection());

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return MapReaderToPatient(reader);
                }
            }

            return null;
        }

        public IEnumerable<Patient> GetAll()
        {
            var patients = new List<Patient>();
            var command = SqlCommandFactory.CreateGetAllPatientsCommand(_dbConnection.GetConnection());
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    patients.Add(MapReaderToPatient(reader));
                }
            }

            return patients;
        }

        public void Update(Patient patient)
        {

            var command = SqlCommandFactory.CreateUpdatePatientCommand(patient, _dbConnection.GetConnection());
            command.ExecuteNonQuery();

        }

        public void Delete(Guid id)
        {

            var command = SqlCommandFactory.CreateDeletePatientCommand(id, _dbConnection.GetConnection());

            command.ExecuteNonQuery();

        }

        public async Task<IEnumerable<Patient>> GetPatientsBySpecificationAsync(ISpecification<Patient> specification)
        {
            var patients = new List<Patient>();

            var command = SqlCommandFactory.CreateGetPatientsBySpecificationCommand(specification, _dbConnection.GetConnection());
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    patients.Add(MapReaderToPatient(reader));
                }
            }
            return patients;
        }


        private Patient MapReaderToPatient(SqlDataReader reader)
        {
            return new Patient(
                reader.GetGuid(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetDateTime(3),
                new PatientContact(reader.GetString(4), reader.GetString(5), reader.GetString(6)),
                (byte[])reader["SocialInsuranceNumber"],
                (byte[])reader["EncryptionIV"]
            );
        }
    }
}