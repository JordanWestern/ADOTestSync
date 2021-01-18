namespace Services
{
    public interface IAzureConfiguration
    {
        string Domain { get; }
        string Organisation { get; }
        string PersonalAccessToken { get; }
        string Project { get; }
    }
}