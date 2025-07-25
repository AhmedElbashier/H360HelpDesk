//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.IO;
//using System.Text;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Cryptography;
//using Microsoft.Net.Http.Headers;
//using LicenseGeneratorApi.Models;

//namespace LicenseGeneratorApi.Controllers
//{
//    [Route("api/v1/[controller]")]
//    [ApiController]
//    public class LicenseControllerTest : ControllerBase
//    {
//        [HttpPost]
//        public IActionResult GenerateLicense([FromBody] LicenseRequest request)
//        {
//            // Validate the request (e.g., user limit)
//            if (request.AdminsLimit <= 0)
//            {
//                return BadRequest("User limit must be greater than zero.");
//            }
//            if (request.AdminsLimit <= 0)
//            {
//                return BadRequest("User limit must be greater than zero.");
//            }
//            // Generate a unique license key (you can use any logic you prefer)
//            string licenseKey = Guid.NewGuid().ToString("N");

//            // Generate a unique license key
//            string licenseKey = Guid.NewGuid().ToString("N");
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Net.Http.Headers;
using LicenseGeneratorApi.Models;

namespace LicenseGeneratorApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LicenseController : ControllerBase
    {
        [HttpPost]
        public IActionResult GenerateLicense([FromBody] LicenseRequest request)
        {
            // Validate the request (e.g., user limit)
            if (request.AdminsLimit <= 0)
            {
                return BadRequest("User limit must be greater than zero.");
            }
            if (request.AdminsLimit <= 0)
            {
                return BadRequest("User limit must be greater than zero.");
            }
            // Generate a unique license key
            string licenseKey = Guid.NewGuid().ToString("N");


            // Create a license object
            var license = new License
            {
                Key = licenseKey,
                Company = request.Company,
                Vendor = request.Vendor,
                AdminsLimit = request.AdminsLimit,
                AgentsLimit = request.AgentsLimit,
                SupervisorsLimit = request.SupervisorsLimit,
                BackOfficeLimit = request.BackOfficeLimit,
                ExpirationDate = DateTime.UtcNow.AddYears(1), // License expires in 1 year
            };



            //            string hardwareId = ComputeHardwareId(request.MacAddress ?? request.Secret);

            //            // Create a license object
            //            var license = new License
            //            {
            //                Key = licenseKey,
            //                Company = request.Company,
            //                Vendor = request.Vendor,
            //                AdminsLimit = request.AdminsLimit,
            //                AgentsLimit = request.AgentsLimit,
            //                SupervisorsLimit = request.SupervisorsLimit,
            //                BackOfficeLimit = request.BackOfficeLimit,
            //                ExpirationDate = DateTime.UtcNow.AddYears(1) // Example: License expires in 1 year
            //                ExpirationDate = DateTime.UtcNow.AddYears(1), // License expires in 1 year
            //                HardwareId = hardwareId
            //            };

            //            // Serialize the license object to a .lic file content
            //            string licenseContent = SerializeLicense(license);

            //            // Set the response headers for a file download
            //            var contentDisposition = new ContentDispositionHeaderValue("attachment")
            //            {
            //                FileName = "mylicense.lic"
            //            };
            //            Response.Headers.Add(HeaderNames.ContentDisposition, contentDisposition.ToString());

            //            // Return the license content as a file
            //            return File(Encoding.UTF8.GetBytes(licenseContent), "application/octet-stream");
            //        }

            //        private string SerializeLicense(License license)
            //        {
            //            // Serialize the license object to a string (you can use any serialization method)
            //            // Here, we are using JSON serialization for simplicity
            //            string serializedLicense = Newtonsoft.Json.JsonConvert.SerializeObject(license);
            //            return serializedLicense;
            //        }
            //    }

            //    public class LicenseRequest
            //    {
            //        public string Vendor { get; set; }
            //        public string Company { get; set; }
            //        public int AdminsLimit { get; set; }
            //        public int AgentsLimit { get; set; }
            //        public int SupervisorsLimit { get; set; }
            //        public int BackOfficeLimit { get; set; }
            //    }
            return Ok();
        }
        private string SerializeLicense(License license)
        {
            // Serialize the license object to a string (you can use any serialization method)
            // Here, we are using JSON serialization for simplicity
            string serializedLicense = Newtonsoft.Json.JsonConvert.SerializeObject(license);
            return serializedLicense;
        }

        private string ComputeHardwareId(string? source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(source));
            return Convert.ToBase64String(hash);
        }
    }

    public class LicenseRequest
    {
        public string Vendor { get; set; }
        public string Company { get; set; }
        public int AdminsLimit { get; set; }
        public int AgentsLimit { get; set; }
        public int SupervisorsLimit { get; set; }
        public int BackOfficeLimit { get; set; }
        public string? MacAddress { get; set; }
        public string? Secret { get; set; }
    }
            //        private string ComputeHardwareId(string? source)
            //        {
            //            if (string.IsNullOrEmpty(source))
            //                return string.Empty;
            //            using var sha = SHA256.Create();
            //            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(source));
            //            return Convert.ToBase64String(hash);
            //        }
            //    }


            //}
        }
