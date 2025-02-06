using ChickenBroker.Api.Auth;
using ChickenBroker.Api.Mapping.Contracts;
using ChickenBroker.Application.Services;
using ChickenBroker.Application.Services.Interfaces;
using ChickenBroker.Contracts.Requests.PropertySold;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;


namespace ChickenBroker.Api.Controllers;
[Authorize(AuthConstants.TrustedMemberPolicyName)]
[ApiController]
public class PropertySoldController : ControllerBase
{
    private readonly IPropertySoldService _propertySoldService;

    public PropertySoldController(IPropertySoldService propertySoldService)
    {
        _propertySoldService = propertySoldService;
    }

    [HttpPost(ApiEndpoints.PropertySold.Create)]
    public async Task<IActionResult> Create([FromBody] CreatePropertySoldRequest request)
    {
        var userId = HttpContext.GetUserId();
        var property = request.MapToPropertySold((Guid)userId!);
        await _propertySoldService.CreateAsync(property, (Guid)userId);
        return CreatedAtAction(nameof(Get), new { id = property.Id }, property);
    }
    
    
    [HttpGet(ApiEndpoints.PropertySold.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var userId = HttpContext.GetUserId();
        var result = await  _propertySoldService.GetByIdAsync(id, userId);
        if (result is null)
        {
            return NotFound();
        }
        return Ok(result.MapToResponse());
    }

    [HttpGet(ApiEndpoints.PropertySold.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllPropertySoldRequest request)
    {
        var userId = HttpContext.GetUserId();
        var options = request.MapToOptions(userId);
        var result = await _propertySoldService.GetAllAsync(options);
        return Ok(result.MapToResponse(1, 1, 10));
    }

    [HttpPut(ApiEndpoints.PropertySold.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id,[FromBody] UpdatePropertySoldRequest request)
    {
        var userId = HttpContext.GetUserId();
        if (userId is null)
        {
            return BadRequest();
        }
        var property = request.MapToPropertySold(id);
        var result = await _propertySoldService.UpdateAsync(property, (Guid)userId);
        if (result is null)
        {
            return NotFound();
        }
        return Ok(result.MapToResponse());
    }

    [HttpDelete(ApiEndpoints.PropertySold.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var userId = HttpContext.GetUserId();
        if (userId is null)
        {
            return BadRequest();
        }
        var result = await _propertySoldService.DeleteByIdAsync(id, (Guid)userId);
        if (!result)
        {
            return NotFound();
        }
        return Ok();
    }
    
}