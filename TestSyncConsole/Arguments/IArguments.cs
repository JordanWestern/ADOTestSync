namespace TestSyncConsole
{
    public interface IArguments
    {
        string Organisation { get; set; }
        string PersonalAccessToken { get; set; }
        string Project { get; set; }
        string ProxyHost { get; set; }
        string ProxyPassword { get; set; }
        int ProxyPort { get; set; }
        string ProxyUsername { get; set; }
    }
}