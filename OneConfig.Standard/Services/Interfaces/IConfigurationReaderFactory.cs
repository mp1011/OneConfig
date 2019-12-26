namespace OneConfig.Services.Interfaces
{
    public interface IConfigurationReaderFactory
    {
        IConfigurationReader TryParseReader(string text);
    }
}
