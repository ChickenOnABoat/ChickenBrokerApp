using ChickenBroker.Api.Mapping.Contracts;
using ChickenBroker.Application.Services.Interfaces;
using ChickenBroker.Contracts.Requests.Address;
using Microsoft.AspNetCore.Mvc;

namespace ChickenBroker.Api.Controllers;

[ApiController]
public class AddressController : ControllerBase
{
    
    private readonly IAddressService _addressService;

    public AddressController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    [HttpPost(ApiEndpoints.Address.Create)]
    public async Task<IActionResult> Create([FromBody] CreateAddressRequest request)
    {
        var alreadyExists = await _addressService.ExistsByZipCodeAsync(request.ZipCode);
        if (alreadyExists)
        {
            return Conflict();
        }
        var result = await _addressService.CreateAsync(request.ZipCode);
        
        return CreatedAtAction(nameof(Get), new { Id = result.Id}, result);
    }

    [HttpGet(ApiEndpoints.Address.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        var addresses = await _addressService.GetAllAsync();
        return Ok(addresses);
    }

    [HttpGet(ApiEndpoints.Address.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await _addressService.GetByIdAsync(id);
        if (result is null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPut(ApiEndpoints.Address.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id,[FromBody] UpdateAddressRequest request)
    {
        var address = request.MapToAddress(id);
        var result = await _addressService.UpdateAsync(address);
        if (result is null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpDelete(ApiEndpoints.Address.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await _addressService.DeleteByIdAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return Ok();
    }
}