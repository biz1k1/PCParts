using System.ComponentModel.DataAnnotations;

namespace PCParts.Domain.Entities;

public class PendingUser
{
    [Key] public Guid Id { get; set; }

    [MaxLength(20)] public string Phone { get; set; }

    public string PasswordHash { get; set; }

    [MaxLength(20)] public string? SmsCode { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}