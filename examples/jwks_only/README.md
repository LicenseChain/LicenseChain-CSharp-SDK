# JWKS-only license assertion verify (C#)

Verifies a **`license_token`** using **`LicenseAssertion.VerifyLicenseAssertionJwtAsync`** against **`GET /v1/licenses/jwks`**. Claim **`token_use`** must be **`licensechain_license_v1`**.

## Run

From this directory:

```bash
dotnet run --project jwks_only.csproj -- "eyJ..." 
```

Or with a custom JWKS URL as the second argument. Environment variables:

| Variable | Purpose |
|----------|---------|
| `LICENSE_TOKEN` | JWT string (if not passed as first argument) |
| `LICENSE_JWKS_URI` | Default `https://api.licensechain.app/v1/licenses/jwks` |
| `LICENSE_JWT_ISSUER` | If set, issuer validation is enforced |
| `EXPECTED_APP_ID` | If set, `aud` must match |
