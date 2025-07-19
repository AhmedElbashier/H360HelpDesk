using System.ComponentModel.DataAnnotations;

namespace LicenseGeneratorApi.Models
{
    public class LicenseRequest
    {
        [Required]
        public string Vendor { get; set; } = string.Empty;

        [Required]
        public string Company { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Must be greater than 0")]
        public int AdminsLimit { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Must be greater than 0")]
        public int AgentsLimit { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Must be greater than 0")]
        public int SupervisorsLimit { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Must be greater than 0")]
        public int BackOfficeLimit { get; set; }

        public string? MacAddress { get; set; }
        public string? Secret { get; set; }
    }
}
