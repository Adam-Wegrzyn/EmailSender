using CsvHelper;
using EmailSender.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender
{
    public static class CsvValidator
    {
        static public bool Validate(FileStream file)
        {
            using var reader = new StreamReader(file);
            using CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<CsvModel>().ToArray();

            if (csv.HeaderRecord == null)
            {
                throw new EmptyFileException("File cannot be empty!");
            }

            if (csv.HeaderRecord[0] == "ToEmail" && records.Length == 0)
            {
                throw new RecipientNotFoundException("At least one recipient must be provided!");
            }

            return true;

        }
    }
}
