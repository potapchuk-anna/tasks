using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstTask.HelpFileManager
{
    internal class TempFileManager
    {
        public static void WriteTemp(string path, int number)
        {
            File.WriteAllText(path, number.ToString());
        }
        public static int GetTemp(string path)
        {
            return int.Parse(File.ReadAllText(path));
        }
        public static void DeleteTemp(string path)
        {
            if(File.Exists(path))
            File.Delete(path);
        }
    }
}
