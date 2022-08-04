using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstTask
{
    abstract class FileReader
    {
        int InvalidLines { get; set; }
        protected abstract CsvConfiguration CreateConfig();

        public List<Model> ReadData(string path)
        {
            List<Model> models = new List<Model>();
            var config = CreateConfig();
            using (var reader = new StreamReader(path))
            using (var csv = new CsvHelper.CsvReader(reader, config))
            {
                while (csv.Read())
                {
                    try
                    {
                        var model = ParseData(csv);
                        models.Add(model);
                    }
                    catch
                    {

                        continue;
                    }
                }
            }
            return models;
        }
        public Model ParseData(CsvHelper.CsvReader csv)
        {
            string address = csv.GetField(2);
            string[] addressComponents;
            if (string.IsNullOrEmpty(address)) throw new Exception("Adress is not valid.");
            else
            {
                addressComponents = address.Split(",", StringSplitOptions.RemoveEmptyEntries);
            }
            var model = new Model
            {
                FirstName = csv.GetField(0),
                LastName = csv.GetField(1),
                Payment = csv.GetField<decimal>(3),
                Date = DateTime.ParseExact(csv.GetField(4), "yyyy-dd-MM", CultureInfo.InvariantCulture),
                AccountNumber = csv.GetField<long>(5),
                Service = csv.GetField(6),
            };
            if (string.IsNullOrEmpty(model.FirstName) ||
                string.IsNullOrEmpty(model.LastName)) throw new Exception("Name or Surname is not valid.");
            model.Address = new Address(addressComponents[0],
                addressComponents[1],
                int.Parse(addressComponents[2]));
            return model;
        }
    }
}
