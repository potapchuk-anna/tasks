using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstTask.Models
{
    internal class Address
    {
        public string City { get; set; }
        public string Street { get; set; }
        public int FlatNumber { get; set; }
        public Address(string city, string street, int flatNumber)
        {
            City = city;
            Street = street;
            FlatNumber = flatNumber;
        }
    }
}
