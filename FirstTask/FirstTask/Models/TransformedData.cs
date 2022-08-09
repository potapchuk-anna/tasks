using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstTask.Models
{
    internal class TransformedData
    {
        public string City { get; set; }
        public List<Service> Services { get; set; } = new List<Service>();
        public decimal Total { get; set; }
    }
}
