namespace Solution.Application.Configuration;

public static class ApplicationSectionNames
{
    public const string Concurrency = Options.ConcurrencyOptions.SectionName;

    public const string Session = Options.AuthSessionOptions.SectionName;

    public const string Storage = Options.StorageOptions.SectionName;
}
