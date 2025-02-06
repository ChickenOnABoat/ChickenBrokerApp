using ChickenBroker.Api.Mapping.Contracts;
using ChickenBroker.Application.Services;
using ChickenBroker.Application.Services.Interfaces;
using ChickenBroker.Contracts.Requests.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ChickenBroker.Api.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    [AllowAnonymous]
    [HttpPost(ApiEndpoints.User.Create)]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var user = request.MapToUser();
        await _userService.CreateAsync(user);
        return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
    }
    [HttpGet(ApiEndpoints.User.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var user = await  _userService.GetByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }
        return Ok(user.MapToResponse());
    }

    [HttpGet(ApiEndpoints.User.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var result = await _userService.GetAllAsync(cancellationToken);
        return Ok(result.MapToResponse());
    }
 }