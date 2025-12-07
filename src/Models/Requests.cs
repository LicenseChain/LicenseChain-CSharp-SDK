using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LicenseChain.CSharp.SDK.Models
{
    // Authentication Requests
    public class UserRegistrationRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class LoginRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }
    }

    public class TokenRefreshResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }

    public class UserUpdateRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("metadata")]
        public Dictionary<string, object> Metadata { get; set; }

        public UserUpdateRequest()
        {
            Metadata = new Dictionary<string, object>();
        }
    }

    public class PasswordChangeRequest
    {
        [JsonProperty("current_password")]
        public string CurrentPassword { get; set; }

        [JsonProperty("new_password")]
        public string NewPassword { get; set; }
    }

    public class PasswordResetRequest
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("new_password")]
        public string NewPassword { get; set; }
    }

    // Application Requests
    public class ApplicationCreateRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("metadata")]
        public Dictionary<string, object> Metadata { get; set; }

        public ApplicationCreateRequest()
        {
            Metadata = new Dictionary<string, object>();
        }
    }

    public class ApplicationListRequest
    {
        [JsonProperty("page")]
        public int? Page { get; set; }

        [JsonProperty("limit")]
        public int? Limit { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("sort_by")]
        public string SortBy { get; set; }

        [JsonProperty("sort_order")]
        public string SortOrder { get; set; }
    }

    public class ApplicationUpdateRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("metadata")]
        public Dictionary<string, object> Metadata { get; set; }

        public ApplicationUpdateRequest()
        {
            Metadata = new Dictionary<string, object>();
        }
    }

    // License Requests
    public class LicenseListRequest
    {
        [JsonProperty("app_id")]
        public string? AppId { get; set; }

        [JsonProperty("page")]
        public int? Page { get; set; }

        [JsonProperty("limit")]
        public int? Limit { get; set; }

        [JsonProperty("status")]
        public string? Status { get; set; }

        [JsonProperty("user_id")]
        public string? UserId { get; set; }

        [JsonProperty("user_email")]
        public string? UserEmail { get; set; }

        [JsonProperty("product_id")]
        public string? ProductId { get; set; }

        [JsonProperty("sort_by")]
        public string? SortBy { get; set; }

        [JsonProperty("sort_order")]
        public string? SortOrder { get; set; }
    }

    // Webhook Requests
    public class WebhookCreateRequest
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("events")]
        public List<string> Events { get; set; }

        [JsonProperty("secret")]
        public string Secret { get; set; }

        public WebhookCreateRequest()
        {
            Events = new List<string>();
        }
    }

    public class WebhookListRequest
    {
        [JsonProperty("app_id")]
        public string? AppId { get; set; }

        [JsonProperty("page")]
        public int? Page { get; set; }

        [JsonProperty("limit")]
        public int? Limit { get; set; }

        [JsonProperty("status")]
        public string? Status { get; set; }

        [JsonProperty("sort_by")]
        public string? SortBy { get; set; }

        [JsonProperty("sort_order")]
        public string? SortOrder { get; set; }
    }

    public class WebhookUpdateRequest
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("events")]
        public List<string> Events { get; set; }

        [JsonProperty("secret")]
        public string Secret { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        public WebhookUpdateRequest()
        {
            Events = new List<string>();
        }
    }

    // Analytics Requests
    public class AnalyticsRequest
    {
        [JsonProperty("app_id")]
        public string? AppId { get; set; }

        [JsonProperty("start_date")]
        public string? StartDate { get; set; }

        [JsonProperty("end_date")]
        public string? EndDate { get; set; }

        [JsonProperty("metric")]
        public string? Metric { get; set; }

        [JsonProperty("period")]
        public string? Period { get; set; }

        [JsonProperty("group_by")]
        public string? GroupBy { get; set; }
    }

    public class UsageStatsRequest
    {
        [JsonProperty("app_id")]
        public string? AppId { get; set; }

        [JsonProperty("start_date")]
        public DateTime? StartDate { get; set; }

        [JsonProperty("end_date")]
        public DateTime? EndDate { get; set; }

        [JsonProperty("period")]
        public string? Period { get; set; }

        [JsonProperty("granularity")]
        public string? Granularity { get; set; }

        [JsonProperty("user_id")]
        public string? UserId { get; set; }

        [JsonProperty("product_id")]
        public string? ProductId { get; set; }
    }
}

