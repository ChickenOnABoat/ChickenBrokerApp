using ChickenBroker.Application.Services;
using ChickenBroker.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChickenBroker.Api.Controllers;

[ApiController]
public class PropertyTypeController : ControllerBase
{
    private readonly IPropertyTypeService _propertyTypeService;

    public PropertyTypeController(IPropertyTypeService propertyTypeService)
    {
        _propertyTypeService = propertyTypeService;
    }


    [HttpGet(ApiEndpoints.PropertyType.GetAll)]
    public async Task<IActionResult> GetPropertyTypes()
    {
        var properties = await _propertyTypeService.GetAllAsync();
        return Ok(properties);
    }
}