using DocumentProcessor.davetn657.Data;
using DocumentProcessor.davetn657.Services;
using Spectre.Console;

namespace DocumentProcessor.davetn657.Views;

public class UserInterface
{
    private readonly string _filePath;
    private readonly IFileReaderService _fileReader;
    private readonly IExportDataService _exporter;
    private readonly PhonebookContext _dbContext;

    public UserInterface(string filePath, FileReaderService fileReader, ExportDataService exporter, PhonebookContext dbContext)
    {
        _filePath = filePath;
        _fileReader = fileReader;
        _exporter = exporter;
        _dbContext = dbContext;
    }

    public void Start()
    {
        while (true)
        {
            TitleCard("Main Menu");
            try
            {
                var fileNames = Directory.GetFiles(_filePath)
                    .Where(s => s.EndsWith(".xlsx") || s.EndsWith(".csv"));

                var menuOptions = new List<string>()
                {
                    "Exit",
                    "Export"
                };

                foreach (var file in fileNames)
                {
                    menuOptions.Add(Path.GetFileName(file));
                }

                AnsiConsole.WriteLine("All files in DocFiles directory");
                var selected = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(menuOptions));

                if (selected.Equals("Exit")) return;

                if (selected.Equals("Export")) ExportData();
                else FileDetails(selected);
            }
            catch (DirectoryNotFoundException ex)
            {

                AnsiConsole.WriteLine($"Couldn't find filePath {_filePath}");
                AnsiConsole.WriteLine("Error: " + ex);
                AnsiConsole.WriteLine("Try entering a new path?");

                var selected = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(new string[] { "Yes", "No" }));

                if (selected.Equals("No")) return;

                var newFilePath = AnsiConsole.Ask<string>("Enter here:");
            }
        }
    }

    public void FileDetails(string fileName)
    {
        TitleCard(fileName + " Details");

        var table = new Table();
        var properties = _fileReader.FormatFile(_filePath, fileName);

        table.AddColumns(new string[]{
            "Name",
            "Email",
            "Phone Number"
        });

        for(int i = 0; i < 10 && 10 < properties.Count; i++)
        {
            table.AddRow(properties[i].Name, properties[i].PhoneNumber, properties[i].Email);
        }

        AnsiConsole.WriteLine("Top Excel Rows:");
        AnsiConsole.Write(table);

        var menuOptions = new List<string> 
        {
            "Import Data",
            "Return"
        };

        var selected = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(menuOptions));

        if (selected.Equals("Return")) return;

        _dbContext.Contacts.AddRange(properties);
        _dbContext.SaveChanges();

        AnsiConsole.WriteLine("Successfully imported data!");
        AnsiConsole.Prompt(new TextPrompt<string>("Return?"));
    }

    public void ExportData()
    {
        TitleCard("Export Current Data");

        var menuOptions = new Dictionary<string, Action?>()
        {
            { "Return", null },
            { ".xlsx", _exporter.ExportToXlsx },
            { ".csv", _exporter.ExportToCsv }
        };

        var selected = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(menuOptions.Keys));

        if (selected.Equals("Return")) return;

        if (menuOptions.TryGetValue(selected, out var action))
        {
            action();
            AnsiConsole.WriteLine("Successfully exported file");
        }
    }

    public void TitleCard(string title)
    {
        var titleCard = new FigletText(title)
        {
            Justification = Justify.Center,
            Color = Color.Blue1
        };

        AnsiConsole.Clear();
        AnsiConsole.Write(titleCard);
    }
}