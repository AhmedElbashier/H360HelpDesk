using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;

[Route("api/v1/[controller]")]
[ApiController]
public class ProxyController : ControllerBase
{
    private readonly IHttpClientFactory _clientFactory;

    public ProxyController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    // GET api/proxy/getPolData
    [HttpGet("getPolData")]
    public async Task<IActionResult> GetPolicyData([FromQuery] int P_LOOKUP_TYPE, [FromQuery] string P_MOBILE)
    {
        var customerApiUrl = $"https://portal.burujinsurance.com:1450/api/getPolData?P_LOOKUP_TYPE={P_LOOKUP_TYPE}&P_MOBILE={P_MOBILE}";

        var client = _clientFactory.CreateClient();

        // Set up basic authentication
        var byteArray = Encoding.ASCII.GetBytes("call_center:cc@p@s$WrD");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

        var response = await client.GetAsync(customerApiUrl);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }
        else
        {
            // This will capture the actual response status code and content
            var errorContent = await response.Content.ReadAsStringAsync();
            return StatusCode((int)response.StatusCode, errorContent);
        }
    }
}
