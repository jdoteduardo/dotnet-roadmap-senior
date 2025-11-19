using ECommerce.OrderManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.OrderManagement.Application.Interfaces
{
    public interface IAddressService
    {
        Task<IEnumerable<AddressDTO>> GetAllAddressesAsync();
        Task<AddressDTO?> GetAddressByIdAsync(int id);
        Task<AddressDTO> CreateAddressAsync(AddressDTO addressDTO);
        Task<AddressDTO> UpdateAddressAsync(AddressDTO addressDTO);
        Task<bool> DeleteAddressAsync(int id);
    }
}
