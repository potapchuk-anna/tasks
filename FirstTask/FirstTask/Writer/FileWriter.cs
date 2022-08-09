using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using FirstTask.Models;

namespace FirstTask.Writer
{
    internal class FileWriter
    {
        public static void WriteOutputFile(List<TransformedData> datas, string path)
        {
            string json = JsonSerializer.Serialize(datas, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(path, json);
        }
    }
}
