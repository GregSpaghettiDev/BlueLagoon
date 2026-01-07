using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Common.Csv
{
    public static class CsvConverter
    {
        public static List<T> Convert<T>(IFormFile file, string delimiter = ";") where T : class
        {
            List<T> records = new();

            using (MemoryStream ms = new MemoryStream())
            {
                file.CopyToAsync(ms);
                ms.Position = 0;
                TextReader textReader = new StreamReader(ms);
                var config = new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = delimiter };

                var csv = new CsvReader(textReader, config);

                records = csv.GetRecords<T>().ToList();

            }

            return records;
        }
    }
}
