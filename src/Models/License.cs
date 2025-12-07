using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LicenseChain.CSharp.SDK.Models
{
    public class License
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("product_id")]
        public string ProductId { get; set; }

        [JsonProperty("license_key")]
        public string LicenseKey { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("expires_at")]
        public DateTime? ExpiresAt { get; set; }

        [JsonProperty("metadata")]
        public Dictionary<string, object> Metadata { get; set; }

        public License()
        {
            Metadata = new Dictionary<string, object>();
        }
    }

    public class CreateLicenseRequest
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("product_id")]
        public string ProductId { get; set; }

        [JsonProperty("metadata")]
        public Dictionary<string, object> Metadata { get; set; }

        public CreateLicenseRequest()
        {
            Metadata = new Dictionary<string, object>();
        }
    }

    public class UpdateLicenseRequest
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("expires_at")]
        public DateTime? ExpiresAt { get; set; }

        [JsonProperty("metadata")]
        public Dictionary<string, object> Metadata { get; set; }

        public UpdateLicenseRequest()
        {
            Metadata = new Dictionary<string, object>();
        }
    }

    public class LicenseListResponse
    {
        [JsonProperty("data")]
        public List<License> Data { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        public LicenseListResponse()
        {
            Data = new List<License>();
        }
    }

    public class LicenseStats
    {
        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("active")]
        public int Active { get; set; }

        [JsonProperty("expired")]
        public int Expired { get; set; }

        [JsonProperty("revoked")]
        public int Revoked { get; set; }

        [JsonProperty("revenue")]
        public decimal Revenue { get; set; }
    }
}
