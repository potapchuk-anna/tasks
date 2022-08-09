using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstTask.Models
{
    internal class Model
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address Address { get; set; }
        public decimal Payment { get; set; }
        public DateTime Date { get; set; }
        public long AccountNumber { get; set; }
        public string Service { get; set; }
    }
}
