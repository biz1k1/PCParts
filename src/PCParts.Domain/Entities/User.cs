using System.ComponentModel.DataAnnotations;

namespace PCParts.Domain.Entities;

public class User
{
    [Key] public Guid Id { get; set; }
    [MaxLength(20)] public string Phone { get; set; }
    public string PasswordHash { get; set; }
}