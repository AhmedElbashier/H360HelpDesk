namespace LicenseGeneratorApi.Models
{
    public class License
    {
        public string Key { get; set; }
        public string Company { get; set; }
        public string Vendor { get; set; }
        public int AdminsLimit { get; set; }
        public int AgentsLimit { get; set; }
        public int SupervisorsLimit { get; set; }
        public int BackOfficeLimit { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}