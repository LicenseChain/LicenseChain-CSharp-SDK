using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using LicenseChain.Exceptions;
using LicenseChain.Models;

namespace LicenseChain
{
    /// <summary>
    /// Main client for interacting with the LicenseChain API
    /// </summary>
    public class LicenseChainClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string _apiKey;
        private readonly JsonSerializerSettings _jsonSettings;
        private bool _disposed = false;

        /// <summary>
        /// Initializes a new instance of the LicenseChainClient class
        /// </summary>
        /// <param name="apiKey">Your LicenseChain API key</param>
        /// <param name="baseUrl">Base URL for the LicenseChain API (optional)</param>
        /// <param name="timeout">Request timeout in seconds (optional, default: 30)</param>
        public LicenseChainClient(string apiKey, string baseUrl = "https://api.licensechain.app", int timeout = 30)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException("API key is required", nameof(apiKey));

            _apiKey = apiKey;
            _baseUrl = baseUrl.TrimEnd('/');
            _jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl),
                Timeout = TimeSpan.FromSeconds(timeout)
            };

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "LicenseChain-CSharp-SDK/1.0.0");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        // Authentication Methods

        /// <summary>
        /// Register a new user
        /// </summary>
        public async Task<User> RegisterUserAsync(UserRegistrationRequest request)
        {
            return await PostAsync<User>("/auth/register", request);
        }

        /// <summary>
        /// Login with email and password
        /// </summary>
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            return await PostAsync<LoginResponse>("/auth/login", request);
        }

        /// <summary>
        /// Logout the current user
        /// </summary>
        public async Task LogoutAsync()
        {
            await PostAsync<object>("/auth/logout", null);
        }

        /// <summary>
        /// Refresh authentication token
        /// </summary>
        public async Task<TokenRefreshResponse> RefreshTokenAsync(string refreshToken)
        {
            var request = new { refresh_token = refreshToken };
            return await PostAsync<TokenRefreshResponse>("/auth/refresh", request);
        }

        /// <summary>
        /// Get current user profile
        /// </summary>
        public async Task<User> GetUserProfileAsync()
        {
            return await GetAsync<User>("/auth/me");
        }

        /// <summary>
        /// Update user profile
        /// </summary>
        public async Task<User> UpdateUserProfileAsync(UserUpdateRequest request)
        {
            return await PatchAsync<User>("/auth/me", request);
        }

        /// <summary>
        /// Change user password
        /// </summary>
        public async Task ChangePasswordAsync(PasswordChangeRequest request)
        {
            await PatchAsync<object>("/auth/password", request);
        }

        /// <summary>
        /// Request password reset
        /// </summary>
        public async Task RequestPasswordResetAsync(string email)
        {
            var request = new { email };
            await PostAsync<object>("/auth/forgot-password", request);
        }

        /// <summary>
        /// Reset password with token
        /// </summary>
        public async Task ResetPasswordAsync(PasswordResetRequest request)
        {
            await PostAsync<object>("/auth/reset-password", request);
        }

        // Application Management

        /// <summary>
        /// Create a new application
        /// </summary>
        public async Task<Application> CreateApplicationAsync(ApplicationCreateRequest request)
        {
            return await PostAsync<Application>("/apps", request);
        }

        /// <summary>
        /// List applications with pagination
        /// </summary>
        public async Task<PaginatedResponse<Application>> ListApplicationsAsync(ApplicationListRequest request)
        {
            var queryParams = new Dictionary<string, string>();
            if (request.Page.HasValue) queryParams["page"] = request.Page.Value.ToString();
            if (request.Limit.HasValue) queryParams["limit"] = request.Limit.Value.ToString();
            if (!string.IsNullOrEmpty(request.Status)) queryParams["status"] = request.Status;
            if (!string.IsNullOrEmpty(request.SortBy)) queryParams["sort_by"] = request.SortBy;
            if (!string.IsNullOrEmpty(request.SortOrder)) queryParams["sort_order"] = request.SortOrder;

            return await GetAsync<PaginatedResponse<Application>>("/apps", queryParams);
        }

        /// <summary>
        /// Get application details
        /// </summary>
        public async Task<Application> GetApplicationAsync(string appId)
        {
            return await GetAsync<Application>($"/apps/{appId}");
        }

        /// <summary>
        /// Update application
        /// </summary>
        public async Task<Application> UpdateApplicationAsync(string appId, ApplicationUpdateRequest request)
        {
            return await PatchAsync<Application>($"/apps/{appId}", request);
        }

        /// <summary>
        /// Delete application
        /// </summary>
        public async Task DeleteApplicationAsync(string appId)
        {
            await DeleteAsync($"/apps/{appId}");
        }

        /// <summary>
        /// Regenerate API key for application
        /// </summary>
        public async Task<ApiKeyResponse> RegenerateApiKeyAsync(string appId)
        {
            return await PostAsync<ApiKeyResponse>($"/apps/{appId}/regenerate-key", null);
        }

        // License Management

        /// <summary>
        /// Create a new license
        /// </summary>
        public async Task<License> CreateLicenseAsync(LicenseCreateRequest request)
        {
            return await PostAsync<License>("/licenses", request);
        }

        /// <summary>
        /// List licenses with filters
        /// </summary>
        public async Task<PaginatedResponse<License>> ListLicensesAsync(LicenseListRequest request)
        {
            var queryParams = new Dictionary<string, string>();
            if (request.Page.HasValue) queryParams["page"] = request.Page.Value.ToString();
            if (request.Limit.HasValue) queryParams["limit"] = request.Limit.Value.ToString();
            if (!string.IsNullOrEmpty(request.AppId)) queryParams["app_id"] = request.AppId;
            if (!string.IsNullOrEmpty(request.Status)) queryParams["status"] = request.Status;
            if (!string.IsNullOrEmpty(request.UserId)) queryParams["user_id"] = request.UserId;
            if (!string.IsNullOrEmpty(request.UserEmail)) queryParams["user_email"] = request.UserEmail;
            if (!string.IsNullOrEmpty(request.SortBy)) queryParams["sort_by"] = request.SortBy;
            if (!string.IsNullOrEmpty(request.SortOrder)) queryParams["sort_order"] = request.SortOrder;

            return await GetAsync<PaginatedResponse<License>>("/licenses", queryParams);
        }

        /// <summary>
        /// Get license details
        /// </summary>
        public async Task<License> GetLicenseAsync(string licenseId)
        {
            return await GetAsync<License>($"/licenses/{licenseId}");
        }

        /// <summary>
        /// Update license
        /// </summary>
        public async Task<License> UpdateLicenseAsync(string licenseId, LicenseUpdateRequest request)
        {
            return await PatchAsync<License>($"/licenses/{licenseId}", request);
        }

        /// <summary>
        /// Delete license
        /// </summary>
        public async Task DeleteLicenseAsync(string licenseId)
        {
            await DeleteAsync($"/licenses/{licenseId}");
        }

        /// <summary>
        /// Validate a license key
        /// </summary>
        public async Task<LicenseValidationResult> ValidateLicenseAsync(string licenseKey, string appId = null)
        {
            var request = new { license_key = licenseKey, app_id = appId };
            return await PostAsync<LicenseValidationResult>("/licenses/validate", request);
        }

        /// <summary>
        /// Revoke a license
        /// </summary>
        public async Task RevokeLicenseAsync(string licenseId, string reason = null)
        {
            var request = new { reason };
            await PatchAsync<object>($"/licenses/{licenseId}/revoke", request);
        }

        /// <summary>
        /// Activate a license
        /// </summary>
        public async Task ActivateLicenseAsync(string licenseId)
        {
            await PatchAsync<object>($"/licenses/{licenseId}/activate", null);
        }

        /// <summary>
        /// Extend license expiration
        /// </summary>
        public async Task ExtendLicenseAsync(string licenseId, string expiresAt)
        {
            var request = new { expires_at = expiresAt };
            await PatchAsync<object>($"/licenses/{licenseId}/extend", request);
        }

        // Webhook Management

        /// <summary>
        /// Create a webhook
        /// </summary>
        public async Task<Webhook> CreateWebhookAsync(WebhookCreateRequest request)
        {
            return await PostAsync<Webhook>("/webhooks", request);
        }

        /// <summary>
        /// List webhooks
        /// </summary>
        public async Task<PaginatedResponse<Webhook>> ListWebhooksAsync(WebhookListRequest request)
        {
            var queryParams = new Dictionary<string, string>();
            if (request.Page.HasValue) queryParams["page"] = request.Page.Value.ToString();
            if (request.Limit.HasValue) queryParams["limit"] = request.Limit.Value.ToString();
            if (!string.IsNullOrEmpty(request.AppId)) queryParams["app_id"] = request.AppId;
            if (!string.IsNullOrEmpty(request.Status)) queryParams["status"] = request.Status;

            return await GetAsync<PaginatedResponse<Webhook>>("/webhooks", queryParams);
        }

        /// <summary>
        /// Get webhook details
        /// </summary>
        public async Task<Webhook> GetWebhookAsync(string webhookId)
        {
            return await GetAsync<Webhook>($"/webhooks/{webhookId}");
        }

        /// <summary>
        /// Update webhook
        /// </summary>
        public async Task<Webhook> UpdateWebhookAsync(string webhookId, WebhookUpdateRequest request)
        {
            return await PatchAsync<Webhook>($"/webhooks/{webhookId}", request);
        }

        /// <summary>
        /// Delete webhook
        /// </summary>
        public async Task DeleteWebhookAsync(string webhookId)
        {
            await DeleteAsync($"/webhooks/{webhookId}");
        }

        /// <summary>
        /// Test webhook
        /// </summary>
        public async Task TestWebhookAsync(string webhookId)
        {
            await PostAsync<object>($"/webhooks/{webhookId}/test", null);
        }

        // Analytics

        /// <summary>
        /// Get analytics data
        /// </summary>
        public async Task<Analytics> GetAnalyticsAsync(AnalyticsRequest request)
        {
            var queryParams = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(request.AppId)) queryParams["app_id"] = request.AppId;
            if (!string.IsNullOrEmpty(request.StartDate)) queryParams["start_date"] = request.StartDate;
            if (!string.IsNullOrEmpty(request.EndDate)) queryParams["end_date"] = request.EndDate;
            if (!string.IsNullOrEmpty(request.Metric)) queryParams["metric"] = request.Metric;
            if (!string.IsNullOrEmpty(request.Period)) queryParams["period"] = request.Period;

            return await GetAsync<Analytics>("/analytics", queryParams);
        }

        /// <summary>
        /// Get license analytics
        /// </summary>
        public async Task<Analytics> GetLicenseAnalyticsAsync(string licenseId)
        {
            return await GetAsync<Analytics>($"/licenses/{licenseId}/analytics");
        }

        /// <summary>
        /// Get usage statistics
        /// </summary>
        public async Task<UsageStats> GetUsageStatsAsync(UsageStatsRequest request)
        {
            var queryParams = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(request.AppId)) queryParams["app_id"] = request.AppId;
            if (!string.IsNullOrEmpty(request.Period)) queryParams["period"] = request.Period;
            if (!string.IsNullOrEmpty(request.Granularity)) queryParams["granularity"] = request.Granularity;

            return await GetAsync<UsageStats>("/analytics/usage", queryParams);
        }

        // System Status

        /// <summary>
        /// Get system status
        /// </summary>
        public async Task<SystemStatus> GetSystemStatusAsync()
        {
            return await GetAsync<SystemStatus>("/status");
        }

        /// <summary>
        /// Get health check
        /// </summary>
        public async Task<HealthCheck> GetHealthCheckAsync()
        {
            return await GetAsync<HealthCheck>("/health");
        }

        // HTTP Methods

        private async Task<T> GetAsync<T>(string endpoint, Dictionary<string, string> queryParams = null)
        {
            var url = endpoint;
            if (queryParams != null && queryParams.Count > 0)
            {
                var queryString = new StringBuilder("?");
                foreach (var param in queryParams)
                {
                    queryString.Append($"{Uri.EscapeDataString(param.Key)}={Uri.EscapeDataString(param.Value)}&");
                }
                url += queryString.ToString().TrimEnd('&');
            }

            var response = await _httpClient.GetAsync(url);
            return await HandleResponseAsync<T>(response);
        }

        private async Task<T> PostAsync<T>(string endpoint, object data)
        {
            var json = data != null ? JsonConvert.SerializeObject(data, _jsonSettings) : "{}";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(endpoint, content);
            return await HandleResponseAsync<T>(response);
        }

        private async Task<T> PatchAsync<T>(string endpoint, object data)
        {
            var json = data != null ? JsonConvert.SerializeObject(data, _jsonSettings) : "{}";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync(endpoint, content);
            return await HandleResponseAsync<T>(response);
        }

        private async Task DeleteAsync(string endpoint)
        {
            var response = await _httpClient.DeleteAsync(endpoint);
            await HandleResponseAsync<object>(response);
        }

        private async Task<T> HandleResponseAsync<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(content);
                throw CreateException(response.StatusCode, errorResponse);
            }

            if (typeof(T) == typeof(object) || string.IsNullOrEmpty(content))
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(content, _jsonSettings);
        }

        private Exception CreateException(System.Net.HttpStatusCode statusCode, ErrorResponse errorResponse)
        {
            var message = errorResponse?.Error ?? "An error occurred";
            var code = errorResponse?.Code;
            var details = errorResponse?.Details;

            return statusCode switch
            {
                System.Net.HttpStatusCode.BadRequest => new ValidationException(message, code, details),
                System.Net.HttpStatusCode.Unauthorized or System.Net.HttpStatusCode.Forbidden => new AuthenticationException(message, code, details),
                System.Net.HttpStatusCode.NotFound => new NotFoundException(message, code, details),
                System.Net.HttpStatusCode.TooManyRequests => new RateLimitException(message, code, details),
                >= System.Net.HttpStatusCode.InternalServerError => new ServerException(message, code, details),
                _ => new LicenseChainException(message, code, details)
            };
        }

        /// <summary>
        /// Dispose the client
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _httpClient?.Dispose();
                _disposed = true;
            }
        }
    }
}
