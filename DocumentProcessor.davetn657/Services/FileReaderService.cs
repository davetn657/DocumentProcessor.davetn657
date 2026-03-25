using DocumentProcessor.davetn657.Models;
using IronXL;
using Spectre.Console;

namespace DocumentProcessor.davetn657.Services;

public interface IFileReaderService
{
    public WorkSheet? ReadXlsxFile(string filePath);
    public List<PhonebookProperties> FormatFile(string filePath, string fileName);
}

public class FileReaderService : IFileReaderService
{
    public FileReaderService()
    {

    }

    public WorkSheet? ReadXlsxFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("File doesn't exist");
            return null;
        }

        var workBook = WorkBook.Load(filePath);
        var workSheet = workBook.WorkSheets.First();

        return workSheet;
    }

    public List<PhonebookProperties> FormatFile(string filePath, string fileName)
    {
        var file = fileName.Split('.');
        var fileType = file[1];
        var properties = new List<PhonebookProperties>();

        switch (fileType)
        {
            case "xlsx" or "csv":
                var workSheet = ReadXlsxFile($"{filePath}\\{fileName}");
                if (workSheet == null)
                {
                    AnsiConsole.WriteLine("File is empty or could not open!");
                    AnsiConsole.Prompt(new TextPrompt<string>("Return?").AllowEmpty());
                }
                properties = FormatToSheetData(workSheet);
                break;
            case "doc":
                break;
            case "pdf":
                break;
        }

        return properties;
    }

    private List<PhonebookProperties> FormatToSheetData(WorkSheet sheetData)
    {
        var properties = new List<PhonebookProperties>();
        var rowCount = sheetData.RowCount;

        for (var i = 2; i < rowCount; i++)
        {
            try
            {
                var data = new PhonebookProperties
                {
                    Name = sheetData["A" + i].TryGetValue<string>(out var name) ? name : string.Empty,
                    PhoneNumber = sheetData["B" + i].TryGetValue<string>(out var phone) ? phone : string.Empty,
                    Email = sheetData["C" + i].TryGetValue<string>(out var email) ? email : string.Empty,
                    Category = sheetData["D" + i].TryGetValue<string>(out var category) ? category : string.Empty,
                };

                properties.Add(data);
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteLine("Could not create property! spreadsheet format is not correct!");
                AnsiConsole.WriteLine(ex.Message);
                AnsiConsole.Prompt(new TextPrompt<string>("Return").AllowEmpty());
                break;
            }
        }

        return properties;
    }
}