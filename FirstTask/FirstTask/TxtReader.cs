using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;

namespace FirstTask
{
    internal class TxtReader : FileReader
    {
        public TxtReader(string path) : base(path)
        {

        }
        protected override CsvConfiguration CreateConfig()
        {
            return new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                TrimOptions = TrimOptions.Trim
            };
        }
    }
}
