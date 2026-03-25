using DocumentProcessor.davetn657.Views;

namespace DocumentProcessor.davetn657;

internal class Program
{
    static void Main(string[] args)
    {
        var directory = Directory.GetCurrentDirectory();
        var userInterface = new UserInterface($"{directory}\\DocFiles");
        userInterface.Start();
    }
}