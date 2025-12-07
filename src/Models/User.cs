using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LicenseChain.CSharp.SDK.Models
{
    public class User
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("metadata")]
        public Dictionary<string, object> Metadata { get; set; }

        [JsonProperty("subscriptions")]
        public List<object>? Subscriptions { get; set; }

        [JsonProperty("variables")]
        public Dictionary<string, object>? Variables { get; set; }

        [JsonProperty("data")]
        public Dictionary<string, object>? Data { get; set; }

        public User()
        {
            Metadata = new Dictionary<string, object>();
            Subscriptions = new List<object>();
            Variables = new Dictionary<string, object>();
            Data = new Dictionary<string, object>();
        }
    }

    public class CreateUserRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("metadata")]
        public Dictionary<string, object> Metadata { get; set; }

        public CreateUserRequest()
        {
            Metadata = new Dictionary<string, object>();
        }
    }

    public class UpdateUserRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("metadata")]
        public Dictionary<string, object> Metadata { get; set; }

        public UpdateUserRequest()
        {
            Metadata = new Dictionary<string, object>();
        }
    }

    public class UserListResponse
    {
        [JsonProperty("data")]
        public List<User> Data { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        public UserListResponse()
        {
            Data = new List<User>();
        }
    }

    public class UserStats
    {
        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("active")]
        public int Active { get; set; }

        [JsonProperty("inactive")]
        public int Inactive { get; set; }
    }
}
