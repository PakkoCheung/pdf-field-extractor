using System;
using System.IO;
using Newtonsoft.Json;

namespace PdfFieldExtractor
{
    public class AppSettings
    {
        public string TableName { get; set; } = "PdfFields";
        public string FieldName { get; set; } = "FieldName";
        public string SqlMode { get; set; } = "Insert"; // "Insert" or "Update"
        public string[] UpdateUniqueKeys { get; set; } = Array.Empty<string>();
    }

    public static class AppConfig
    {
        public static AppSettings Settings { get; private set; } = new AppSettings();

        public static void Load(string? path = null)
        {
            path ??= Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            if (!File.Exists(path)) return;

            try
            {
                var json = File.ReadAllText(path);
                var settings = JsonConvert.DeserializeObject<AppSettings>(json);
                if (settings != null) Settings = settings;
            }
            catch
            {
                // ignore parse errors and keep defaults
            }
        }
    }
}
