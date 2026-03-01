namespace PdfFieldExtractor;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        // Load runtime configuration from appsettings.json before initializing UI
        AppConfig.Load();

        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());
    }
}
