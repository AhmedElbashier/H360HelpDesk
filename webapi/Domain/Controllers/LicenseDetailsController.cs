using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IO;

namespace webapi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LicenseDetailsController : ControllerBase
    {
        [HttpGet("file")]
        public IActionResult GetLicenseFile()
        {
            try
            {
                // Define the path to your license file
                string licenseFilePath = "license.lic";

                if (System.IO.File.Exists(licenseFilePath))
                {
                    // Read the license file as bytes
                    byte[] licenseFileBytes = System.IO.File.ReadAllBytes(licenseFilePath);

                    // Return the license file to the end-user as a downloadable file
                    return File(licenseFileBytes, "application/octet-stream", "license.lic");
                }
                else
                {
                    // Handle the case where the license file is missing
                    return BadRequest("License file not found.");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during file reading
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("details")]
        public IActionResult GetLicenseDetails()
        {
            try
            {
                // Define the path to your license file
                string licenseFilePath = "license.lic";

                if (System.IO.File.Exists(licenseFilePath))
                {
                    // Read the license file as bytes
                    byte[] licenseFileBytes = System.IO.File.ReadAllBytes(licenseFilePath);

                    // Read the license file content as JSON
                    string json = System.IO.File.ReadAllText(licenseFilePath);
                    License licenseObject = JsonConvert.DeserializeObject<License>(json);

                    // Create a custom response model
                    var responseModel = new
                    {
                        LicenseFile = licenseFileBytes,
                        LicenseInfo = licenseObject
                    };

                    // Serialize the response model to JSON and return it
                    return Ok(responseModel);
                }
                else
                {
                    // Handle the case where the license file is missing
                    return BadRequest("License file not found.");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during file reading
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
