namespace PCParts.Application.Model.Models;

public class PendingUser
{
    public Guid Id { get; set; }
    public string Phone { get; set; }
    public string PasswordHash { get; set; }
}