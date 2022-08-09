using FirstTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstTask.DataTransformer
{
    internal class DataTransformer
    {
        public static List<TransformedData> Transform(List<Model> models)
        {
            return models.GroupBy(m => m.Address.City)
                .Select(m => new TransformedData
                {
                    City = m.Key,
                    Services = m.GroupBy(s => s.Service).Select(s => new Service
                    {
                        Name = s.Key,
                        Payers = s.Select(p => new Payer
                        {
                            Name = p.FirstName + " " + p.LastName,
                            Payment = p.Payment,
                            Date = p.Date,
                            AccountNumber = p.AccountNumber
                        }).ToList(),
                        Total = s.Sum(p => p.Payment)
                    }).ToList(),
                    Total = m.Sum(s => s.Payment)
                }).ToList();

        }
    }
}
