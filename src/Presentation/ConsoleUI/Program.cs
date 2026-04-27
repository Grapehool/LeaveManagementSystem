using ConsoleUI.App;

class Program
{
    static async Task Main(string[] args)
    {
        var app = new AppRunner();
        await app.Run();
    }
}