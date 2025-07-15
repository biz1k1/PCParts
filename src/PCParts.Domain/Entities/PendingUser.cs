using System.ComponentModel.DataAnnotations;

namespace PCParts.Domain.Entities;

public class PendingUser
{
    [Key] public Guid Id { get; set; }
    [MaxLength(20)] public string Email { get; set; }
    public string PasswordHash { get; set; }
    public bool EmailConfirmed { get; set; }
    public string? EmailConfirmationToken { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}