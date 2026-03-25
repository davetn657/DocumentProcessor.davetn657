using DocumentProcessor.davetn657.Services;
using Spectre.Console;

namespace DocumentProcessor.davetn657.Views;

public class UserInterface
{
    private readonly string _filePath;
    private readonly IFileReaderService _fileReader;

    public UserInterface(string filePath, FileReaderService fileReader)
    {
        _filePath = filePath;
        _fileReader = fileReader;
    }

    public void Start()
    {
        while (true)
        {
            TitleCard("Main Menu");
            try
            {
                var fileNames = Directory.GetFiles(_filePath)
                    .Where(s => s.EndsWith(".xlsx") || s.EndsWith(".doc") || s.EndsWith(".pdf") || s.EndsWith(".csv"));

                var menuOptions = new List<string>()
                {
                    "Exit"
                };

                foreach (var file in fileNames)
                {
                    menuOptions.Add(Path.GetFileName(file));
                }

                AnsiConsole.WriteLine("All files in DocFiles directory");
                var selected = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(menuOptions));

                if (selected.Equals("Exit")) return;

                FileDetails(selected);
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