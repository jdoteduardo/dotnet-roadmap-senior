using AutoMapper;
using ECommerce.OrderManagement.Application.DTOs;
using ECommerce.OrderManagement.Application.Interfaces;
using ECommerce.OrderManagement.Domain.Entities;
using ECommerce.OrderManagement.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.OrderManagement.Application.Services
{
    public class AddressService : IAddressService
    {
        private readonly IRepository<Address> _addressRepository;
        private readonly IMapper _mapper;

        public AddressService(IRepository<Address> addressRepository, IMapper mapper)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AddressDTO>> GetAllAddressesAsync()
        {
            var addresses = await _addressRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<AddressDTO>>(addresses);
        }

        public async Task<AddressDTO?> GetAddressByIdAsync(int id)
        {
            var address = await _addressRepository.GetByIdAsync(id);

            return _mapper.Map<AddressDTO?>(address);
        }

        public async Task<AddressDTO> CreateAddressAsync(AddressDTO addressDTO)
        {
            var address = _mapper.Map<Address>(addressDTO);
            var createdAddress = await _addressRepository.AddAsync(address);
            return _mapper.Map<AddressDTO>(createdAddress);
        }

        public Task<AddressDTO> UpdateAddressAsync(AddressDTO addressDTO)
        {
            var address = _mapper.Map<Address>(addressDTO);
            var createdAddress =  _addressRepository.UpdateAsync(address);
            return _mapper.Map<Task<AddressDTO>>(createdAddress);
        }

        public async Task<bool> DeleteAddressAsync(int id)
        {
            return await _addressRepository.DeleteAsync(id);
        }
    }
}
