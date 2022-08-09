using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser;

namespace FirstTask.Reader
{
    internal class CsvReader : FileReader
    {
        public CsvReader(string path) : base(path)
        {

        }
        protected override CsvConfiguration CreateConfig()
        {
            return new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                TrimOptions = TrimOptions.Trim
            };
        }

    }
}
