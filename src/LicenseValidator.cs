using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace LicenseChainSDK
{
    public class LicenseValidator
    {
        private readonly string apiUrl;

        public LicenseValidator(string apiUrl)
        {
            this.apiUrl = apiUrl;
        }

        public async Task<bool> ValidateLicenseAsync(string licenseKey)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync($"{apiUrl}/validate?licenseKey=" + licenseKey);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return content == "valid";
                }
                return false;
            }
        }
    }
}
