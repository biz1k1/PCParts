using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCParts.Domain.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }
        public bool PhoneConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
