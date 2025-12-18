using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LicenseChain.CSharp.SDK.Exceptions;
using LicenseChain.CSharp.SDK.Models;
using LicenseChain.CSharp.SDK.Utils;
using static LicenseChain.CSharp.SDK.Utils.Utils;
using Newtonsoft.Json;

namespace LicenseChain.CSharp.SDK.Services
{
    public class LicenseService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public LicenseService(HttpClient httpClient, string apiKey, string baseUrl)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;
            _baseUrl = baseUrl;
        }

        public async Task<License> CreateLicenseAsync(CreateLicenseRequest request)
        {
            ValidateNotEmpty(request.UserId, nameof(request.UserId));
            ValidateNotEmpty(request.ProductId, nameof(request.ProductId));

            request.Metadata = SanitizeMetadata(request.Metadata);

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync($"{_baseUrl}/licenses", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<ApiResponse<License>>(responseContent);
                    return result.Data;
                }

                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);
                throw new LicenseChainException(
                    errorResponse?.ErrorCode ?? "LICENSE_CREATE_ERROR",
                    errorResponse?.Message ?? $"Failed to create license: {responseContent}",
                    (int)response.StatusCode
                );
            }
            catch (HttpRequestException ex)
            {
                throw new NetworkException("Network error occurred while creating license", ex);
            }
        }

        public async Task<License> GetLicenseAsync(string licenseId)
        {
            ValidateNotEmpty(licenseId, nameof(licenseId));
            ValidateUuid(licenseId);

            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/licenses/{licenseId}");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<ApiResponse<License>>(responseContent);
                    return result.Data;
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new NotFoundException($"License with ID {licenseId} not found");
                }

                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);
                throw new LicenseChainException(
                    errorResponse?.ErrorCode ?? "LICENSE_GET_ERROR",
                    errorResponse?.Message ?? $"Failed to get license: {responseContent}",
                    (int)response.StatusCode
                );
            }
            catch (HttpRequestException ex)
            {
                throw new NetworkException("Network error occurred while getting license", ex);
            }
        }

        public async Task<License> UpdateLicenseAsync(string licenseId, UpdateLicenseRequest request)
        {
            ValidateNotEmpty(licenseId, nameof(licenseId));
            ValidateUuid(licenseId);

            request.Metadata = SanitizeMetadata(request.Metadata);

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PutAsync($"{_baseUrl}/licenses/{licenseId}", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<ApiResponse<License>>(responseContent);
                    return result.Data;
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new NotFoundException($"License with ID {licenseId} not found");
                }

                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);
                throw new LicenseChainException(
                    errorResponse?.ErrorCode ?? "LICENSE_UPDATE_ERROR",
                    errorResponse?.Message ?? $"Failed to update license: {responseContent}",
                    (int)response.StatusCode
                );
            }
            catch (HttpRequestException ex)
            {
                throw new NetworkException("Network error occurred while updating license", ex);
            }
        }

        public async Task RevokeLicenseAsync(string licenseId)
        {
            ValidateNotEmpty(licenseId, nameof(licenseId));
            ValidateUuid(licenseId);

            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/licenses/{licenseId}");

                if (!response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        throw new NotFoundException($"License with ID {licenseId} not found");
                    }

                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);
                    throw new LicenseChainException(
                        errorResponse?.ErrorCode ?? "LICENSE_REVOKE_ERROR",
                        errorResponse?.Message ?? $"Failed to revoke license: {responseContent}",
                        (int)response.StatusCode
                    );
                }
            }
            catch (HttpRequestException ex)
            {
                throw new NetworkException("Network error occurred while revoking license", ex);
            }
        }

        public async Task<bool> ValidateLicenseAsync(string licenseKey)
        {
            ValidateNotEmpty(licenseKey, nameof(licenseKey));

            // Use /licenses/verify endpoint with 'key' parameter to match API
            var request = new { key = licenseKey };
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync($"{_baseUrl}/licenses/verify", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<ApiResponse<ValidationResult>>(responseContent);
                    return result.Data.Valid;
                }

                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);
                throw new LicenseChainException(
                    errorResponse?.ErrorCode ?? "LICENSE_VALIDATE_ERROR",
                    errorResponse?.Message ?? $"Failed to validate license: {responseContent}",
                    (int)response.StatusCode
                );
            }
            catch (HttpRequestException ex)
            {
                throw new NetworkException("Network error occurred while validating license", ex);
            }
        }

        public async Task<LicenseListResponse> ListUserLicensesAsync(string userId, int page = 1, int limit = 10)
        {
            ValidateNotEmpty(userId, nameof(userId));
            ValidateUuid(userId);

            var (validPage, validLimit) = ValidatePagination(page, limit);

            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/licenses?user_id={userId}&page={validPage}&limit={validLimit}");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<LicenseListResponse>(responseContent);
                }

                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);
                throw new LicenseChainException(
                    errorResponse?.ErrorCode ?? "LICENSE_LIST_ERROR",
                    errorResponse?.Message ?? $"Failed to list user licenses: {responseContent}",
                    (int)response.StatusCode
                );
            }
            catch (HttpRequestException ex)
            {
                throw new NetworkException("Network error occurred while listing user licenses", ex);
            }
        }

        public async Task<LicenseStats> GetLicenseStatsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/licenses/stats");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<ApiResponse<LicenseStats>>(responseContent);
                    return result.Data;
                }

                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);
                throw new LicenseChainException(
                    errorResponse?.ErrorCode ?? "LICENSE_STATS_ERROR",
                    errorResponse?.Message ?? $"Failed to get license stats: {responseContent}",
                    (int)response.StatusCode
                );
            }
            catch (HttpRequestException ex)
            {
                throw new NetworkException("Network error occurred while getting license stats", ex);
            }
        }
    }

    public class ApiResponse<T>
    {
        [JsonProperty("data")]
        public T Data { get; set; }
    }

    public class ValidationResult
    {
        [JsonProperty("valid")]
        public bool Valid { get; set; }
    }
}
