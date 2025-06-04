using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using webapi.Domain.Helpers;

[Route("api/v1/[controller]")]
[ApiController]
public class ProxyController : ControllerBase
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ExternalApisOptions _apiOptions;

    public ProxyController(IHttpClientFactory clientFactory, IOptions<ExternalApisOptions> apiOptions)
    {
        _clientFactory = clientFactory;
        _apiOptions = apiOptions.Value;
    }

    [HttpGet("getPolData")]
    public async Task<IActionResult> GetPolicyData([FromQuery] int P_LOOKUP_TYPE, [FromQuery] string P_MOBILE)
    {
        var customerApiUrl = $"{_apiOptions.PolicyDataUrl}?P_LOOKUP_TYPE={P_LOOKUP_TYPE}&P_MOBILE={P_MOBILE}";

        var client = _clientFactory.CreateClient();
        var byteArray = Encoding.ASCII.GetBytes("call_center:cc@p@s$WrD");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

        var response = await client.GetAsync(customerApiUrl);
        var content = await response.Content.ReadAsStringAsync();

        return response.IsSuccessStatusCode ? Ok(content) : StatusCode((int)response.StatusCode, content);
    }

    [HttpGet("getClaimsData")]
    public async Task<IActionResult> GetClaimsData([FromQuery] int P_LOOKUP_TYPE, [FromQuery] string P_MOBILE)
    {
        var customerApiUrl = $"{_apiOptions.ClaimsDataUrl}?P_LOOKUP_TYPE={P_LOOKUP_TYPE}&P_MOBILE={P_MOBILE}";

        var client = _clientFactory.CreateClient();
        var byteArray = Encoding.ASCII.GetBytes("call_center:cc@p@s$WrD");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

        var response = await client.GetAsync(customerApiUrl);
        var content = await response.Content.ReadAsStringAsync();

        return response.IsSuccessStatusCode ? Ok(content) : StatusCode((int)response.StatusCode, content);
    }
}
