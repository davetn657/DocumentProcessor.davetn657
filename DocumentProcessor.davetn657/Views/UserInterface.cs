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
        TitleCard("Main Menu");

        try
        {
            var fileNames = Directory.GetFiles(_filePath)
                .Where(s => s.EndsWith(".xlsx") || s.EndsWith(".doc") || s.EndsWith(".pdf") || s.EndsWith(".csv"));

            var menuOptions = new List<string>()
            {
                "Exit"
            };

            foreach(var file in fileNames)
            {
                menuOptions.Add(Path.GetFileName(file));   
            }

            AnsiConsole.WriteLine("All files in DocFiles directory");
            var selected = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(menuOptions));

            if (selected.Equals("Exit")) return;

            FileDetails(selected);
        }
        catch(DirectoryNotFoundException ex){

            AnsiConsole.WriteLine($"Couldn't find filePath {_filePath}");
            AnsiConsole.WriteLine("Error: " + ex);
            AnsiConsole.WriteLine("Try entering a new path?");

            var selected = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(new string[] { "Yes", "No" }));

            if (selected.Equals("No")) return;

            var newFilePath = AnsiConsole.Ask<string>("Enter here:");
        }
    }

    public void FileDetails(string fileName)
    {
        TitleCard(fileName + " Details");

        var file = fileName.Split('.');
        var fileType = file[1];

        switch (fileType)
        {
            case "xlsx":
                break;
            case "doc":
                break;
            case "pdf":
                break;
            case "csv":
                break;
        }


    }

    public void TitleCard(string title)
    {
        var titleCard = new FigletText(title)
        {
            Justification = Justify.Center,
            Color = Color.Blue1
        };

        AnsiConsole.Write(titleCard);
    }
}