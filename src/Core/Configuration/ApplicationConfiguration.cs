namespace Toarnbeike.AppLibrary.Configuration;

public sealed class ApplicationConfiguration
{
    public required string Directory { get; set; }

    public string DataDirectory => Path.Combine(Directory, "Data");
    public string LogDirectory => Path.Combine(Directory, "Logs");
    public string ExportDirectory => Path.Combine(Directory, "Export");
}