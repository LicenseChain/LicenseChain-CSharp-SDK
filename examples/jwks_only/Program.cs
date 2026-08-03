// JWKS-only: verify license_token with token + JWKS URI (parity with Go/Rust/PHP jwks_only).
// Env: LICENSECHAIN_LICENSE_TOKEN, LICENSECHAIN_LICENSE_JWKS_URI
// Optional: LICENSECHAIN_EXPECTED_APP_ID
//
// Run: dotnet run --project examples/jwks_only/jwks_only.csproj (from repo root)

using System.Net.Http;
using LicenseChain;

var token = Environment.GetEnvironmentVariable("LICENSECHAIN_LICENSE_TOKEN")?.Trim() ?? "";
var jwks = Environment.GetEnvironmentVariable("LICENSECHAIN_LICENSE_JWKS_URI")?.Trim() ?? "";
if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(jwks))
{
    Console.Error.WriteLine("Set LICENSECHAIN_LICENSE_TOKEN and LICENSECHAIN_LICENSE_JWKS_URI");
    Environment.Exit(1);
}

var opts = new LicenseAssertion.VerifyLicenseAssertionOptions();
var appId = Environment.GetEnvironmentVariable("LICENSECHAIN_EXPECTED_APP_ID");
if (!string.IsNullOrWhiteSpace(appId))
    opts.ExpectedAppId = appId.Trim();

using var http = new HttpClient();
var jwt = await LicenseAssertion.VerifyLicenseAssertionJwtAsync(http, token, jwks, opts).ConfigureAwait(false);
Console.WriteLine(jwt.Payload.SerializeToJson());
