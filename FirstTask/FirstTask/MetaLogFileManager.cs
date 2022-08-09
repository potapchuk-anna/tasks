using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstTask
{
    internal class MetaLogFileManager
    {
        private static string path
        {
            get
            {
               return PathGetter.GetOutputDirectory(DateTime.Now.Date.AddDays(-1).ToShortDateString()) + "/meta.log";
            }
        }         
        public static void WriteMetaLog()
        {
            Dictionary<string, string> temp = new Dictionary<string, string>();
            temp.Add("parsed_files", Convert.ToString(FileReader.PassedFiles));
            temp.Add("parsed_lines", Convert.ToString(FileReader.PassedLines));
            temp.Add("found_errors", Convert.ToString(FileReader.InvalidLines));
            Func<string> func = () =>
            {
                string str = "[";
                foreach (var file in FileReader.InvalidFilesList)
                {
                    str += file + " ";
                }
                str += "]";
                return str;
            };
            temp.Add("invalid_files", func.Invoke());
            List<string> lines = new List<string>();
            foreach(var key in temp.Keys)
            {
                lines.Add(string.Format("{0}: {1}", key, temp[key]));
            }
            File.WriteAllLines(path, lines);
        }
        public static void CreateMetaLog()
        {
           File.Create(path).Close();
        }
        public static bool MetaLogExist()
        {
            return File.Exists(path);
        }
        public static bool HasDateChanged(DateTime date)
        {
            return (DateTime.Today - date.Date).Days > 0;
        }
    }
}
