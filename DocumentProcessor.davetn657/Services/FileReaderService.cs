using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace DocumentProcessor.davetn657.Services;

public interface IFileReaderService
{
    public string ReadXlsxFile(string filePath);
}

public class FileReaderService : IFileReaderService
{
    public FileReaderService()
    {

    }

    public string ReadXlsxFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            Console.WriteLine("File doesn't exist");
            return string.Empty;
        }
        
        using(var document = SpreadsheetDocument.Open(filePath, false))
        {
            var workbookPart = document.WorkbookPart;
            var sheet = workbookPart.Workbook.Descendants<Sheet>().First();
            var worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
            var sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();

            if (!sheetData.Elements<Row>().Any())
            {
                Console.WriteLine("File is Empty");
            }
            else
            {

            }

        }


        return string.Empty;
    }
}