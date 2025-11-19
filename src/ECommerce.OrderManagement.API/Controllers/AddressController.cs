using ECommerce.OrderManagement.Application.DTOs;
using ECommerce.OrderManagement.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ECommerce.OrderManagement.API.Controllers
{
    [Route("api/v1/addresses")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAddresses()
        {
            var addresses = await _addressService.GetAllAddressesAsync();
            return Ok(addresses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddressById(int id)
        {
            var address = await _addressService.GetAddressByIdAsync(id);

            if (address == null)
            {
                return NotFound();
            }

            return Ok(address);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddress([FromBody] AddressDTO addressDTO)
        {
            var createdAddress = await _addressService.CreateAddressAsync(addressDTO);
            return CreatedAtAction(nameof(GetAddressById), new { id = createdAddress.Id }, createdAddress);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddress(int id, [FromBody] AddressDTO addressDTO)
        {
            if (id != addressDTO.Id)
            {
                return BadRequest(new { Message = "The URL ID does not match the address ID." });
            }

            var updatedAddress = await _addressService.UpdateAddressAsync(addressDTO);

            if (updatedAddress == null)
            {
                return NotFound();
            }

            return Ok(updatedAddress);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var result = await _addressService.DeleteAddressAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
