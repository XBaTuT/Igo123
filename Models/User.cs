using System;

namespace Igo123.Models
{
    public class User
    {
        public string Surname { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public Gender Gender { get; set; }
        public DateTime Birthday { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
    }
}