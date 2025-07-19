using LicenseGeneratorApi.Controllers;
using LicenseGeneratorApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using System.Text;
using Newtonsoft.Json;
using System.Security.Cryptography;
using LicenseGeneratorApi.Services;
using System;

namespace LicenseGeneratorApi.Tests
{
    public class LicenseControllerTests
    {
        [Fact]
        public void GenerateLicense_ReturnsBadRequest_WhenAdminsLimitIsZero()
        {
            var controller = new LicenseController(new LicenseService());
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            var request = new LicenseRequest
            {
                Vendor = "V",
                Company = "C",
                AdminsLimit = 0,
                AgentsLimit = 1,
                SupervisorsLimit = 1,
                BackOfficeLimit = 1
            };

            var result = controller.GenerateLicense(request);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("AdminsLimit must be greater than zero.", badRequest.Value);
        }

        [Fact]
        public void GenerateLicense_ReturnsFile_WhenRequestIsValid()
        {
            var controller = new LicenseController(new LicenseService());
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            var request = new LicenseRequest
            {
                Vendor = "V",
                Company = "C",
                AdminsLimit = 1,
                AgentsLimit = 2,
                SupervisorsLimit = 3,
                BackOfficeLimit = 4
            };

            var result = controller.GenerateLicense(request);

            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("application/octet-stream", fileResult.ContentType);
            var json = Encoding.UTF8.GetString(fileResult.FileContents);
            var license = JsonConvert.DeserializeObject<License>(json);
            Assert.Equal(request.Company, license.Company);
            Assert.Equal(request.Vendor, license.Vendor);
            Assert.Equal(request.AdminsLimit, license.AdminsLimit);
        }

        [Fact]
        public void GenerateLicense_IncludesHardwareId_WhenMacAddressProvided()
        {
            var controller = new LicenseController(new LicenseService());
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            var request = new LicenseRequest
            {
                Vendor = "V",
                Company = "C",
                AdminsLimit = 1,
                AgentsLimit = 2,
                SupervisorsLimit = 3,
                BackOfficeLimit = 4,
                MacAddress = "AA-BB-CC-DD-EE-FF"
            };

            var result = controller.GenerateLicense(request);

            var fileResult = Assert.IsType<FileContentResult>(result);
            var license = JsonConvert.DeserializeObject<License>(Encoding.UTF8.GetString(fileResult.FileContents));
            using var sha = SHA256.Create();
            var expectedId = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(request.MacAddress)));
            Assert.Equal(expectedId, license.HardwareId);
        }
    }
}
