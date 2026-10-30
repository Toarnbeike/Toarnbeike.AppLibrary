namespace Toarnbeike.AppLibrary.Configuration;

public class ApplicationConfigurationTests
{
    private readonly ApplicationConfiguration _config = new() { Directory = "BasePath" };

    [Test]
    public void DataDirectory_ShouldReturnCorrectPath()
    {
        var expectedDataDirectory = $"BasePath{Path.DirectorySeparatorChar}Data";
        _config.DataDirectory.ShouldBe(expectedDataDirectory);
    }

    [Test]
    public void LogDirectory_ShouldReturnCorrectPath()
    {
        var expectedLogDirectory = $"BasePath{Path.DirectorySeparatorChar}Logs";
        _config.LogDirectory.ShouldBe(expectedLogDirectory);
    }

    [Test]
    public void ExportDirectory_ShouldReturnCorrectPath()
    {
        var expectedExportDirectory = $"BasePath{Path.DirectorySeparatorChar}Export";
        _config.ExportDirectory.ShouldBe(expectedExportDirectory);
    }
}
