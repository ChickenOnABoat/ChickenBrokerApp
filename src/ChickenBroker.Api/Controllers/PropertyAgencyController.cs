using ChickenBroker.Api.Mapping.Contracts;
using ChickenBroker.Application.Models;
using ChickenBroker.Application.Services;
using ChickenBroker.Application.Services.Interfaces;
using ChickenBroker.Contracts.Requests.PropertyAgency;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ChickenBroker.Api.Controllers;

[ApiController]
public class PropertyAgencyController : ControllerBase
{
    private readonly IPropertyAgencyService _propertyAgencyService;
    private readonly IValidator<PropertyAgency> _propertyAgencyValidator;
    

    public PropertyAgencyController(IPropertyAgencyService propertyAgencyService, IValidator<PropertyAgency> propertyAgencyValidator)
    {
        _propertyAgencyService = propertyAgencyService;
        _propertyAgencyValidator = propertyAgencyValidator;
    }
    
    [HttpPost(ApiEndpoints.PropertyAgency.Create)]
    public async Task<IActionResult> Create([FromBody] CreatePropertyAgencyRequest request, CancellationToken cancellationToken = default)
    {
        var propertyAgency = request.MapToPropertyAgency();
        await _propertyAgencyService.CreateAsync(propertyAgency, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = propertyAgency.Id }, propertyAgency);
        
    }

    [HttpGet(ApiEndpoints.PropertyAgency.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var propertyAgency = await  _propertyAgencyService.GetByIdAsync(id);
        if (propertyAgency is null)
        {
            return NotFound();
        }
        return Ok(propertyAgency.MapToResponse());
    }

    [HttpGet(ApiEndpoints.PropertyAgency.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllPropertyAgencyRequest request,CancellationToken cancellationToken = default)
    {
        var options = request.MapToOptions();
        var propertyAgencies = await _propertyAgencyService.GetAllAsync(options, cancellationToken);
        var totalPropertyAgencies = await _propertyAgencyService.GetCountAllAsync(options.Name, options.City, options.IdentificationDocumentNumber, options.ZipCode, cancellationToken);
        return Ok(propertyAgencies.MapToResponse(request.Page, request.PageSize, totalPropertyAgencies));
    }
    
    [HttpPut(ApiEndpoints.PropertyAgency.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdatePropertyAgencyRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _propertyAgencyService.UpdateAsync(request.MapToPropertyAgency(id), cancellationToken);
        if (result is null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpDelete(ApiEndpoints.PropertyAgency.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _propertyAgencyService.DeleteByIdAsync(id, cancellationToken);
        if (!result)
        {
            return NotFound();  
        }
        return Ok();
    }
}