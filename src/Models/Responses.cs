using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LicenseChain.CSharp.SDK.Models
{
    // Application Model
    public class Application
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("metadata")]
        public Dictionary<string, object> Metadata { get; set; }

        public Application()
        {
            Metadata = new Dictionary<string, object>();
        }
    }

    // License Validation Result
    public class LicenseValidationResult
    {
        [JsonProperty("valid")]
        public bool Valid { get; set; }

        [JsonProperty("license")]
        public License License { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("error_code")]
        public string ErrorCode { get; set; }
    }

    // Analytics Models
    public class Analytics
    {
        [JsonProperty("total_licenses")]
        public int TotalLicenses { get; set; }

        [JsonProperty("active_licenses")]
        public int ActiveLicenses { get; set; }

        [JsonProperty("expired_licenses")]
        public int ExpiredLicenses { get; set; }

        [JsonProperty("revoked_licenses")]
        public int RevokedLicenses { get; set; }

        [JsonProperty("total_revenue")]
        public decimal TotalRevenue { get; set; }

        [JsonProperty("period_revenue")]
        public decimal PeriodRevenue { get; set; }

        [JsonProperty("data")]
        public List<AnalyticsDataPoint> Data { get; set; }

        public Analytics()
        {
            Data = new List<AnalyticsDataPoint>();
        }
    }

    public class AnalyticsDataPoint
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("value")]
        public decimal Value { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }
    }

    public class UsageStats
    {
        [JsonProperty("total_requests")]
        public int TotalRequests { get; set; }

        [JsonProperty("successful_requests")]
        public int SuccessfulRequests { get; set; }

        [JsonProperty("failed_requests")]
        public int FailedRequests { get; set; }

        [JsonProperty("average_response_time")]
        public double AverageResponseTime { get; set; }

        [JsonProperty("data")]
        public List<UsageStatsDataPoint> Data { get; set; }

        public UsageStats()
        {
            Data = new List<UsageStatsDataPoint>();
        }
    }

    public class UsageStatsDataPoint
    {
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("requests")]
        public int Requests { get; set; }

        [JsonProperty("errors")]
        public int Errors { get; set; }
    }

    // System Status Models
    public class SystemStatus
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("uptime")]
        public long Uptime { get; set; }

        [JsonProperty("services")]
        public Dictionary<string, ServiceStatus> Services { get; set; }

        public SystemStatus()
        {
            Services = new Dictionary<string, ServiceStatus>();
        }
    }

    public class ServiceStatus
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("last_check")]
        public DateTime LastCheck { get; set; }
    }

    public class HealthCheck
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
    }

    // API Key Response
    public class ApiKeyResponse
    {
        [JsonProperty("api_key")]
        public string ApiKey { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("expires_at")]
        public DateTime? ExpiresAt { get; set; }
    }

    // Paginated Response
    public class PaginatedResponse<T>
    {
        [JsonProperty("data")]
        public List<T> Data { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        public PaginatedResponse()
        {
            Data = new List<T>();
        }
    }

    // Error Response
    public class ErrorResponse
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("error_code")]
        public string ErrorCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("details")]
        public Dictionary<string, object> Details { get; set; }

        public ErrorResponse()
        {
            Details = new Dictionary<string, object>();
        }
    }
}

