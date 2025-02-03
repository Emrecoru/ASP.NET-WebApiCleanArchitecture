namespace App.Domain.Options
{
    public class ConnectionStringOption
    {
        public const string Key = "ConnectionStrings";
        public string Local { get; set; } = default!;
    }
}
