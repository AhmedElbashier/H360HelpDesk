using System.Security.Cryptography;
using System.Text;
using LicenseGeneratorApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LicenseGeneratorApi.Pages
{
    public class ManageModel : PageModel
    {
        [BindProperty]
        public LicenseRequest Input { get; set; } = new();

        public string? GeneratedLicense { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var license = new License
            {
                Key = Guid.NewGuid().ToString("N"),
                Company = Input.Company,
                Vendor = Input.Vendor,
                AdminsLimit = Input.AdminsLimit,
                AgentsLimit = Input.AgentsLimit,
                SupervisorsLimit = Input.SupervisorsLimit,
                BackOfficeLimit = Input.BackOfficeLimit,
                ExpirationDate = DateTime.UtcNow.AddYears(1),
                HardwareId = ComputeHardwareId(Input.MacAddress ?? Input.Secret)
            };

            GeneratedLicense = SerializeLicense(license);

            return Page();
        }

        private string ComputeHardwareId(string? source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(source));
            return Convert.ToBase64String(hash);
        }

        private string SerializeLicense(License license)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(license);
        }
    }
}
