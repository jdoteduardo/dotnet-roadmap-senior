using ECommerce.OrderManagement.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.OrderManagement.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Email Email { get; set; } = new();
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Order>? Orders { get; set; }
    }
}
