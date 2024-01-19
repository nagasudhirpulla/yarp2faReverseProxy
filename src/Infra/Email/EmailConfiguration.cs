namespace Infra.Services.Email;

public class EmailConfiguration
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Domain { get; set; } = string.Empty;
    public string MailAddress { get; set; } = string.Empty;
    public string HostName { get; set; } = string.Empty;
}
