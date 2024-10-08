using System;
using System.Threading.Tasks;
using LicenseChainSDK;

namespace LicenseChainSDKTests
{
    public class LicenseValidatorTests
    {
        public async Task TestValidateLicenseAsync()
        {
            LicenseValidator validator = new LicenseValidator("https://api.licensechain.com");
            bool isValid = await validator.ValidateLicenseAsync("sample-license-key");

            Console.WriteLine(isValid ? "License is valid." : "License is invalid.");
        }
    }
}
