using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LicenseChain.CSharp.SDK.Models
{
    public class Webhook
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("events")]
        public List<string> Events { get; set; }

        [JsonProperty("secret")]
        public string Secret { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        public Webhook()
        {
            Events = new List<string>();
        }
    }

    public class CreateWebhookRequest
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("events")]
        public List<string> Events { get; set; }

        [JsonProperty("secret")]
        public string Secret { get; set; }

        public CreateWebhookRequest()
        {
            Events = new List<string>();
        }
    }

    public class UpdateWebhookRequest
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("events")]
        public List<string> Events { get; set; }

        [JsonProperty("secret")]
        public string Secret { get; set; }

        public UpdateWebhookRequest()
        {
            Events = new List<string>();
        }
    }

    public class WebhookListResponse
    {
        [JsonProperty("data")]
        public List<Webhook> Data { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        public WebhookListResponse()
        {
            Data = new List<Webhook>();
        }
    }

    public class WebhookEvent
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }
    }
}
