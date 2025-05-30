using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PCParts.Domain.Enum;

namespace PCParts.Domain.Entities;

public class DomainEvents
{
    [Key] public Guid DomainEventId { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DomainEventType Type { get; set; }
    public DateTimeOffset? ActivityAt { get; set; }

    [Required]
    [Column(TypeName = "jsonb")]
    public string Content { get; set; }
}