using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace FirstTask
{
    internal class DataGetter
    {
        private FileReader fileReader;
        public List<List<Model>> models { get; }
        public DataGetter(FileReader fileReader)
        {
            this.fileReader = fileReader;
        }      
        public List<string> GetPathes()
        {            
            try
            {

                return Directory.GetFiles(ConfigurationSettings.AppSettings["input"])
                    .Where(file=>file.EndsWith(".csv")||file.EndsWith(".txt"))
                    .ToList();
            }
            catch
            {
                Directory.CreateDirectory(ConfigurationSettings.AppSettings["input"]);
                return new List<string>();
            }

        }
        public void Process()
        {
            List<string> filePathes = GetPathes();
            List<Task> tasks = new List<Task>();
            foreach(string filePath in filePathes)
            {
                //tasks.Add(ReadAsync(filePath));
                ReadAsync(filePath);
            }
            //await Task.WhenAll(tasks);

        }
        public List<Model> ReadAsync(string path)
        {
             List<Model> models = fileReader.ReadData(path);
            return models;
        }
    }
}
