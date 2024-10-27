using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientProject.Domain.Aggregates.PatientAggregate
{
    public class PatientContact
    {
        public string Address { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }

        public PatientContact(string address, string email, string phoneNumber)
        {
            Address = address;
            Email = email;
            PhoneNumber = phoneNumber;
        }
    }
}
