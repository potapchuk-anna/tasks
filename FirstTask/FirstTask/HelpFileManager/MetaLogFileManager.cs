using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstTask.Reader;
namespace FirstTask.HelpFileManager
{
    internal class MetaLogFileManager
    {
        private int InvalidLines;
        private int PassedFiles;
        private List<string> InvalidFilesList = new List<string>();
        private int PassedLines;
        private string path
        {
            get
            {
                return PathGetter.GetOutputDirectory(DateTime.Now.Date.AddDays(-1).ToShortDateString()) + "/meta.log";
            }
        }
        public void ChangeMetaLogData(FileReader fileReader)
        {
            InvalidLines += fileReader.InvalidLines;
            PassedFiles += fileReader.PassedLines;
            PassedLines += fileReader.PassedLines;
            InvalidFilesList.AddRange(fileReader.InvalidFilesList);
        }
        public void WriteMetaLog()
        {
            Dictionary<string, string> temp = new Dictionary<string, string>();
            temp.Add("parsed_files", Convert.ToString(PassedFiles));
            temp.Add("parsed_lines", Convert.ToString(PassedLines));
            temp.Add("found_errors", Convert.ToString(InvalidLines));
            Func<string> func = () =>
            {
                string str = "[";
                foreach (var file in InvalidFilesList)
                {
                    str += file + " ";
                }
                str += "]";
                return str;
            };
            temp.Add("invalid_files", func.Invoke());
            List<string> lines = new List<string>();
            foreach (var key in temp.Keys)
            {
                lines.Add(string.Format("{0}: {1}", key, temp[key]));
            }
            File.WriteAllLines(path, lines);
            ClearMetaLogFields();
        }
        public void CreateMetaLog()
        {
            File.Create(path).Close();
        }
        public bool MetaLogExist()
        {
            return File.Exists(path);
        }
        public static bool HasDateChanged(DateTime date)
        {
            return (DateTime.Today - date.Date).Days > 0;
        }
        private void ClearMetaLogFields()
        {
            InvalidFilesList.Clear();
            InvalidLines = 0;
            PassedFiles = 0;
            PassedLines = 0;
        }
    }
}
