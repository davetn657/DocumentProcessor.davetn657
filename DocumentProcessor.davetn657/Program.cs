using DocumentProcessor.davetn657.Views;
using DocumentProcessor.davetn657.Data;
using DocumentProcessor.davetn657.Services;

namespace DocumentProcessor.davetn657;

internal class Program
{
    static void Main(string[] args)
    {
        var directory = Directory.GetCurrentDirectory();
        var fileReader = new FileReaderService();
        var dbContext = new PhonebookContext();
        var exporter = new ExportDataService(dbContext);

        if (!dbContext.Contacts.Any())
        {
            var properties = fileReader.FormatFile(Directory.GetCurrentDirectory(), "SeedData.xlsx");
            dbContext.Contacts.AddRange(properties);
            dbContext.SaveChanges();
        }

        var userInterface = new UserInterface($"{directory}\\DocFiles", fileReader, exporter, dbContext);
        userInterface.Start();
    }
}