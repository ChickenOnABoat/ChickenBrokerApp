using Microsoft.AspNetCore.Mvc;
using SDKBrasilAPI;

namespace ChickenBroker.Api.Controllers;

public class TestController : ControllerBase
{
    
    private readonly IBrasilAPI _brasilApi;

    public TestController(IBrasilAPI brasilApi)
    {
        _brasilApi = brasilApi;
    }

    [HttpGet("api/test/{cep}")]
    public async Task<IActionResult> Get([FromRoute] string cep)
    {
        var result = await _brasilApi.CEP_V2(cep);
        var state = await _brasilApi.IBGE_UF(24);
        return Ok(result);
    }
}