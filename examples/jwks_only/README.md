# JWKS-only (`license_token`)

Verifies a Core API **`license_token`** using **`GET /v1/licenses/jwks`** (RS256), without calling `VerifyLicenseWithDetailsAsync` first. Same env contract as **Go / Rust / PHP** `jwks_only` samples ([JWKS_EXAMPLE_PRIORITY](https://github.com/LicenseChain/sdks/blob/main/docs/JWKS_EXAMPLE_PRIORITY.md)).

## Env

| Variable | Required | Description |
|----------|----------|-------------|
| `LICENSECHAIN_LICENSE_TOKEN` | yes | JWT string from a successful verify response |
| `LICENSECHAIN_LICENSE_JWKS_URI` | yes | e.g. `https://api.licensechain.app/v1/licenses/jwks` |
| `LICENSECHAIN_EXPECTED_APP_ID` | no | If set, `aud` must include this app UUID |

## Run

From the **C# SDK repo root**:

```bash
dotnet run --project examples/jwks_only/jwks_only.csproj
```

With env:

```bash
export LICENSECHAIN_LICENSE_TOKEN="eyJ..."
export LICENSECHAIN_LICENSE_JWKS_URI="https://api.licensechain.app/v1/licenses/jwks"
dotnet run --project examples/jwks_only/jwks_only.csproj
```
