using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LicenseChain.CSharp.SDK.Models
{
    public class Product
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("metadata")]
        public Dictionary<string, object> Metadata { get; set; }

        public Product()
        {
            Metadata = new Dictionary<string, object>();
        }
    }

    public class CreateProductRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("metadata")]
        public Dictionary<string, object> Metadata { get; set; }

        public CreateProductRequest()
        {
            Metadata = new Dictionary<string, object>();
        }
    }

    public class UpdateProductRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("price")]
        public decimal? Price { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("metadata")]
        public Dictionary<string, object> Metadata { get; set; }

        public UpdateProductRequest()
        {
            Metadata = new Dictionary<string, object>();
        }
    }

    public class ProductListResponse
    {
        [JsonProperty("data")]
        public List<Product> Data { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        public ProductListResponse()
        {
            Data = new List<Product>();
        }
    }

    public class ProductStats
    {
        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("active")]
        public int Active { get; set; }

        [JsonProperty("revenue")]
        public decimal Revenue { get; set; }
    }
}
