namespace PCParts.Application.Model.Models;

public class User
{
    public Guid Id { get; set; }
    public string Phone { get; set; }
    public bool PhoneConfirmed { get; set; }
    public string PasswordHash { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}