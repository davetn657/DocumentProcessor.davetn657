using DocumentProcessor.davetn657.Views;
using DocumentProcessor.davetn657.Services;

namespace DocumentProcessor.davetn657;

internal class Program
{
    static void Main(string[] args)
    {
        var directory = Directory.GetCurrentDirectory();
        var userInterface = new UserInterface($"{directory}\\DocFiles", new FileReaderService());
        userInterface.Start();
    }
}