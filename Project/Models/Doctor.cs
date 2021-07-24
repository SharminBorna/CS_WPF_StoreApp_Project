using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Doctor
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialties { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string Action { get; set; }
    }
}
