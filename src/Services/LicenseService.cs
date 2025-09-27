using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LicenseChain.CSharp.SDK.Exceptions;
using LicenseChain.CSharp.SDK.Models;
using LicenseChain.CSharp.SDK.Utils;
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
            Utils.ValidateNotEmpty(request.UserId, nameof(request.UserId));
            Utils.ValidateNotEmpty(request.ProductId, nameof(request.ProductId));

            request.Metadata = Utils.SanitizeMetadata(request.Metadata);

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

                throw new LicenseChainException($"Failed to create license: {responseContent}")
                {
                    StatusCode = (int)response.StatusCode
                };
            }
            catch (HttpRequestException ex)
            {
                throw new NetworkException("Network error occurred while creating license", ex);
            }
        }

        public async Task<License> GetLicenseAsync(string licenseId)
        {
            Utils.ValidateNotEmpty(licenseId, nameof(licenseId));
            Utils.ValidateUuid(licenseId);

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

                throw new LicenseChainException($"Failed to get license: {responseContent}")
                {
                    StatusCode = (int)response.StatusCode
                };
            }
            catch (HttpRequestException ex)
            {
                throw new NetworkException("Network error occurred while getting license", ex);
            }
        }

        public async Task<License> UpdateLicenseAsync(string licenseId, UpdateLicenseRequest request)
        {
            Utils.ValidateNotEmpty(licenseId, nameof(licenseId));
            Utils.ValidateUuid(licenseId);

            request.Metadata = Utils.SanitizeMetadata(request.Metadata);

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

                throw new LicenseChainException($"Failed to update license: {responseContent}")
                {
                    StatusCode = (int)response.StatusCode
                };
            }
            catch (HttpRequestException ex)
            {
                throw new NetworkException("Network error occurred while updating license", ex);
            }
        }

        public async Task RevokeLicenseAsync(string licenseId)
        {
            Utils.ValidateNotEmpty(licenseId, nameof(licenseId));
            Utils.ValidateUuid(licenseId);

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

                    throw new LicenseChainException($"Failed to revoke license: {responseContent}")
                    {
                        StatusCode = (int)response.StatusCode
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                throw new NetworkException("Network error occurred while revoking license", ex);
            }
        }

        public async Task<bool> ValidateLicenseAsync(string licenseKey)
        {
            Utils.ValidateNotEmpty(licenseKey, nameof(licenseKey));

            var request = new { license_key = licenseKey };
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync($"{_baseUrl}/licenses/validate", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<ApiResponse<ValidationResult>>(responseContent);
                    return result.Data.Valid;
                }

                throw new LicenseChainException($"Failed to validate license: {responseContent}")
                {
                    StatusCode = (int)response.StatusCode
                };
            }
            catch (HttpRequestException ex)
            {
                throw new NetworkException("Network error occurred while validating license", ex);
            }
        }

        public async Task<LicenseListResponse> ListUserLicensesAsync(string userId, int page = 1, int limit = 10)
        {
            Utils.ValidateNotEmpty(userId, nameof(userId));
            Utils.ValidateUuid(userId);

            var (validPage, validLimit) = Utils.ValidatePagination(page, limit);

            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/licenses?user_id={userId}&page={validPage}&limit={validLimit}");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<LicenseListResponse>(responseContent);
                }

                throw new LicenseChainException($"Failed to list user licenses: {responseContent}")
                {
                    StatusCode = (int)response.StatusCode
                };
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

                throw new LicenseChainException($"Failed to get license stats: {responseContent}")
                {
                    StatusCode = (int)response.StatusCode
                };
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
