using ConsoleUI.App;

class Program
{
    static async Task Main()
    {
        var app = new AppRunner();
        await app.Run();
    }
}