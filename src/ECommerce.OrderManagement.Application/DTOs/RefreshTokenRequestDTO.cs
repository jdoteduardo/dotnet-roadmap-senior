using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.OrderManagement.Application.DTOs
{
    public class RefreshTokenRequestDTO
    {
        [Required(ErrorMessage = "Refresh token é obrigatório")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
