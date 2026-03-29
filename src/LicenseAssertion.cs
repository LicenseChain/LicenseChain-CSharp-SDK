using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace LicenseChain
{
    /// <summary>
    /// RS256 license_token verification via JWKS (parity with Node verifyLicenseAssertionJwt).
    /// </summary>
    public static class LicenseAssertion
    {
        /// <summary>Must match Core API LICENSE_TOKEN_USE_CLAIM.</summary>
        public const string LICENSE_TOKEN_USE_CLAIM = "licensechain_license_v1";

        public sealed class VerifyLicenseAssertionOptions
        {
            public string? ExpectedAppId { get; set; }
            public string? Issuer { get; set; }
        }

        public static async Task<JwtSecurityToken> VerifyLicenseAssertionJwtAsync(
            HttpClient httpClient,
            string token,
            string jwksUrl,
            VerifyLicenseAssertionOptions? options = null)
        {
            options ??= new VerifyLicenseAssertionOptions();
            token = token?.Trim() ?? throw new ArgumentException("empty token", nameof(token));
            jwksUrl = jwksUrl?.Trim() ?? throw new ArgumentException("empty jwksUrl", nameof(jwksUrl));
            if (token.Length == 0)
                throw new ArgumentException("empty token", nameof(token));
            if (jwksUrl.Length == 0)
                throw new ArgumentException("empty jwksUrl", nameof(jwksUrl));

            var jwksJson = await httpClient.GetStringAsync(jwksUrl).ConfigureAwait(false);
            var jwks = new JsonWebKeySet(jwksJson);
            var keys = jwks.GetSigningKeys();

            var handler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKeys = keys,
                ValidateIssuer = !string.IsNullOrWhiteSpace(options.Issuer),
                ValidIssuer = options.Issuer,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(2),
                ValidAlgorithms = new[] { SecurityAlgorithms.RsaSha256 }
            };

            handler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            var jwt = (JwtSecurityToken)validatedToken;

            var tu = jwt.Payload.TryGetValue("token_use", out var tuObj) ? tuObj?.ToString() : null;
            if (tu != LICENSE_TOKEN_USE_CLAIM)
                throw new SecurityTokenException($"Invalid license token: expected token_use \"{LICENSE_TOKEN_USE_CLAIM}\"");

            if (!string.IsNullOrWhiteSpace(options.ExpectedAppId))
            {
                var want = options.ExpectedAppId.Trim();
                if (!jwt.Audiences.Contains(want))
                    throw new SecurityTokenException("Invalid license token: aud does not match expected app id");
            }

            return jwt;
        }
    }
}
