using LicenseChain;

var token = args.Length > 0 ? args[0].Trim() : Environment.GetEnvironmentVariable("LICENSE_TOKEN")?.Trim() ?? "";
var jwksDefault = "https://api.licensechain.app/v1/licenses/jwks";
var jwksEnv = Environment.GetEnvironmentVariable("LICENSE_JWKS_URI")?.Trim();
var jwks = args.Length > 1
    ? args[1].Trim()
    : (string.IsNullOrEmpty(jwksEnv) ? jwksDefault : jwksEnv);

if (string.IsNullOrEmpty(token))
{
    Console.Error.WriteLine("Missing JWT: set LICENSE_TOKEN or pass as first argument.");
    return 1;
}

using var http = new HttpClient();
var opts = new LicenseAssertion.VerifyLicenseAssertionOptions();
var iss = Environment.GetEnvironmentVariable("LICENSE_JWT_ISSUER")?.Trim();
if (!string.IsNullOrEmpty(iss))
    opts.Issuer = iss;
var app = Environment.GetEnvironmentVariable("EXPECTED_APP_ID")?.Trim();
if (!string.IsNullOrEmpty(app))
    opts.ExpectedAppId = app;

var jwt = await LicenseAssertion.VerifyLicenseAssertionJwtAsync(http, token, jwks, opts).ConfigureAwait(false);
Console.WriteLine($"OK token_use={LicenseAssertion.LICENSE_TOKEN_USE_CLAIM} sub={jwt.Subject}");
return 0;
