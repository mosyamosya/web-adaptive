using Bogus;
using ClosedXML.Excel;

namespace WebAdaptive.Services.DataRandomizerService
{
    public class DataRandomizerService : IDataRandomizerService
    {
        private readonly Faker _faker = new Faker();

        //version 1
        public int GetIntegerValue()
        {
            Random random = new Random();
            return random.Next();
        }
        //version 2
        public string GetTextValue()
        {
            string randomText = _faker.Name.FirstName();
            return randomText;
        }

        //version 3
        public byte[] GenerateExcelFile()
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Sheet1");
                    worksheet.Cell("A1").Value = "Hello";
                    worksheet.Cell("B1").Value = "World";
                    worksheet.Cell("A2").Value = "Swansea";
                    worksheet.Cell("B2").Value = "BSNU";
                    workbook.SaveAs(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }
    }
}
