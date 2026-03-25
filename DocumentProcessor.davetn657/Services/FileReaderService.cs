using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace DocumentProcessor.davetn657.Services;

public interface IFileReaderService
{
    public SheetData? ReadXlsxFile(string filePath);
}

public class FileReaderService : IFileReaderService
{
    public FileReaderService()
    {

    }

    public SheetData? ReadXlsxFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            Console.WriteLine("File doesn't exist");
            return null;
        }

        var document = SpreadsheetDocument.Open(filePath, false);
        var workbookPart = document.WorkbookPart;

        if (workbookPart == null) return null;
        var worksheetPart = workbookPart.WorksheetParts.First();

        if (worksheetPart.Worksheet == null) return null;
        var sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();

        return sheetData;
    }
}