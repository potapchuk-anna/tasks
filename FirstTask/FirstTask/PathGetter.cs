using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using FirstTask.HelpFileManager;

namespace FirstTask
{
    internal class PathGetter
    {
        private static readonly object _locker = new object();
        public static bool HasDataChanged { get; private set; } = false;
        public static List<string> GetPathes()
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
        public static string GetOutputDirectory(string date)
        {
            string outputPath = ConfigurationSettings.AppSettings["output"];
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }
            string folderPath = outputPath + "/" + date;
            if (!Directory.Exists(folderPath))
            {
                HasDataChanged = true;
                Directory.CreateDirectory(folderPath);
            }
            else
            {
                HasDataChanged = false;
            }
            return folderPath;
        }
        public static string GetOutputPath()
        {

            string folderPath = GetOutputDirectory(DateTime.Now.ToShortDateString());
            string tempFilePath = folderPath + "/.temp";
            int number;
            lock (_locker)
            {
                if (!File.Exists(tempFilePath))
                {
                    var tempFile = File.Create(tempFilePath);
                    number = 1;
                    tempFile.Close();
                }
                else
                {
                    number = TempFileManager.GetTemp(tempFilePath);
                    number++;
                }

                TempFileManager.WriteTemp(tempFilePath, number);
            }
            
            return folderPath + "/output" + number + ".json";
        }
    }
}
