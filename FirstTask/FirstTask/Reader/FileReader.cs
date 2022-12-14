using CsvHelper.Configuration;
using FirstTask.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstTask.Reader
{
    abstract class FileReader
    {
        StreamReader reader;
        string path;
        public FileReader(string path)
        {
            this.path = path;           
            reader = new StreamReader(path);
        }
        public int InvalidLines { get; protected set; }
        public int PassedFiles { get; protected set; }
        public List<string> InvalidFilesList { get; protected set; } = new List<string>();
        public int PassedLines { get; protected set; }
        protected abstract CsvConfiguration CreateConfig();

        public List<Model> ReadData()
        {
            List<Model> models = new List<Model>();
            var config = CreateConfig();
            using (var csv = new CsvHelper.CsvReader(reader, config))
            {
                while (csv.Read())
                {
                    try
                    {
                        var model = ParseData(csv);
                        PassedLines++;
                        models.Add(model);
                    }
                    catch
                    {
                        InvalidLines++;
                        continue;
                    }
                }
                if (InvalidLines > 0)
                {
                    InvalidFilesList.Add(path);
                }
                else
                {
                    PassedFiles++;
                }
            }

            reader.Close();
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
        public void Close()
        {
            reader.Close();
        }
        public void DeleteFile()
        {
            File.Delete(path);
        }
    }
}
