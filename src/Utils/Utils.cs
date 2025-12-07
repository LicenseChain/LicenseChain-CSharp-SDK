using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace LicenseChain.CSharp.SDK.Utils
{
    public static class Utils
    {
        public static bool ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            try
            {
                var emailRegex = new Regex(@"^[^\s@]+@[^\s@]+\.[^\s@]+$");
                return emailRegex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

        public static bool ValidateLicenseKey(string licenseKey)
        {
            if (string.IsNullOrEmpty(licenseKey))
                return false;

            return licenseKey.Length == 32 && Regex.IsMatch(licenseKey, @"^[A-Z0-9]+$");
        }

        public static bool ValidateUuid(string uuid)
        {
            if (string.IsNullOrEmpty(uuid))
                return false;

            return Guid.TryParse(uuid, out _);
        }

        public static bool ValidateAmount(decimal amount)
        {
            return amount > 0 && amount != decimal.MaxValue && amount != decimal.MinValue;
        }

        public static bool ValidateCurrency(string currency)
        {
            var validCurrencies = new[] { "USD", "EUR", "GBP", "CAD", "AUD", "JPY", "CHF", "CNY" };
            return validCurrencies.Contains(currency?.ToUpper());
        }

        public static string SanitizeInput(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return input
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&#x27;");
        }

        public static Dictionary<string, object> SanitizeMetadata(Dictionary<string, object> metadata)
        {
            if (metadata == null)
                return new Dictionary<string, object>();

            var sanitized = new Dictionary<string, object>();

            foreach (var kvp in metadata)
            {
                if (kvp.Value is string stringValue)
                {
                    sanitized[kvp.Key] = SanitizeInput(stringValue);
                }
                else if (kvp.Value is List<object> listValue)
                {
                    var sanitizedList = listValue.Select(item =>
                        item is string str ? SanitizeInput(str) : item
                    ).ToList();
                    sanitized[kvp.Key] = sanitizedList;
                }
                else if (kvp.Value is Dictionary<string, object> dictValue)
                {
                    sanitized[kvp.Key] = SanitizeMetadata(dictValue);
                }
                else
                {
                    sanitized[kvp.Key] = kvp.Value;
                }
            }

            return sanitized;
        }

        public static string GenerateLicenseKey()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 32)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string GenerateUuid()
        {
            return Guid.NewGuid().ToString();
        }

        public static string FormatTimestamp(DateTime timestamp)
        {
            return timestamp.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture);
        }

        public static DateTime ParseTimestamp(string timestamp)
        {
            return DateTime.Parse(timestamp, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
        }

        public static (int page, int limit) ValidatePagination(int? page, int? limit)
        {
            var validPage = Math.Max(page ?? 1, 1);
            var validLimit = Math.Min(Math.Max(limit ?? 10, 1), 100);
            return (validPage, validLimit);
        }

        public static void ValidateDateRange(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
                throw new ArgumentException("Start date must be before or equal to end date");
        }

        public static string CreateWebhookSignature(string payload, string secret)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        public static bool VerifyWebhookSignature(string payload, string signature, string secret)
        {
            var expectedSignature = CreateWebhookSignature(payload, secret);
            return string.Equals(signature, expectedSignature, StringComparison.OrdinalIgnoreCase);
        }

        public static string RetryWithBackoff<T>(Func<T> func, int maxRetries = 3, int initialDelayMs = 1000)
        {
            var delay = initialDelayMs;
            Exception lastException = null;

            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    func();
                    return null; // Success
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    if (i < maxRetries - 1)
                    {
                        System.Threading.Thread.Sleep(delay);
                        delay *= 2; // Exponential backoff
                    }
                }
            }

            throw lastException;
        }

        public static string FormatBytes(long bytes)
        {
            string[] units = { "B", "KB", "MB", "GB", "TB", "PB" };
            double size = bytes;
            int unitIndex = 0;

            while (size >= 1024 && unitIndex < units.Length - 1)
            {
                size /= 1024;
                unitIndex++;
            }

            return $"{size:F1} {units[unitIndex]}";
        }

        public static string FormatDuration(int seconds)
        {
            if (seconds < 60)
                return $"{seconds}s";
            else if (seconds < 3600)
            {
                var minutes = seconds / 60;
                var remainingSeconds = seconds % 60;
                return $"{minutes}m {remainingSeconds}s";
            }
            else if (seconds < 86400)
            {
                var hours = seconds / 3600;
                var minutes = (seconds % 3600) / 60;
                return $"{hours}h {minutes}m";
            }
            else
            {
                var days = seconds / 86400;
                var hours = (seconds % 86400) / 3600;
                return $"{days}d {hours}h";
            }
        }

        public static string FormatPrice(decimal price, string currency = "USD")
        {
            return price.ToString("C", new CultureInfo($"en-US") { NumberFormat = { CurrencySymbol = GetCurrencySymbol(currency) } });
        }

        private static string GetCurrencySymbol(string currency)
        {
            return currency switch
            {
                "USD" => "$",
                "EUR" => "€",
                "GBP" => "£",
                "JPY" => "¥",
                _ => currency
            };
        }

        public static string CapitalizeFirst(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            return char.ToUpper(text[0]) + text.Substring(1).ToLower();
        }

        public static string ToSnakeCase(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            return Regex.Replace(text, @"([a-z])([A-Z])", "$1_$2").ToLower();
        }

        public static string ToPascalCase(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            return string.Join("", text.Split('_').Select(word => CapitalizeFirst(word)));
        }

        public static string TruncateString(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text) || text.Length <= maxLength)
                return text;

            return text.Substring(0, maxLength - 3) + "...";
        }

        public static string Slugify(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            return text
                .ToLower()
                .Replace(" ", "-")
                .Replace("_", "-")
                .Replace("--", "-")
                .Trim('-');
        }

        public static void ValidateNotEmpty(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"{fieldName} cannot be empty");
        }

        public static void ValidatePositive(decimal value, string fieldName)
        {
            if (value <= 0)
                throw new ArgumentException($"{fieldName} must be positive");
        }

        public static void ValidateRange(decimal value, decimal min, decimal max, string fieldName)
        {
            if (value < min || value > max)
                throw new ArgumentException($"{fieldName} must be between {min} and {max}");
        }

        public static string JsonSerialize(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
        }

        public static T JsonDeserialize<T>(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        public static bool IsValidJson(string jsonString)
        {
            try
            {
                Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidUrl(string urlString)
        {
            return Uri.TryCreate(urlString, UriKind.Absolute, out _);
        }

        public static string UrlEncode(string value)
        {
            return Uri.EscapeDataString(value);
        }

        public static string UrlDecode(string value)
        {
            return Uri.UnescapeDataString(value);
        }

        public static long GetCurrentTimestamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        public static string GetCurrentDate()
        {
            return DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture);
        }

        public static string Sha256(string data)
        {
            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        public static string Sha1(string data)
        {
            using (var sha1 = SHA1.Create())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        public static string Md5(string data)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        public static string Base64Encode(string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            return Convert.ToBase64String(bytes);
        }

        public static string Base64Decode(string data)
        {
            var bytes = Convert.FromBase64String(data);
            return Encoding.UTF8.GetString(bytes);
        }

        public static List<List<T>> ChunkArray<T>(List<T> array, int chunkSize)
        {
            var chunks = new List<List<T>>();
            for (int i = 0; i < array.Count; i += chunkSize)
            {
                chunks.Add(array.GetRange(i, Math.Min(chunkSize, array.Count - i)));
            }
            return chunks;
        }

        public static Dictionary<string, object> DeepMerge(Dictionary<string, object> target, Dictionary<string, object> source)
        {
            var result = new Dictionary<string, object>(target);

            foreach (var kvp in source)
            {
                if (result.ContainsKey(kvp.Key) && result[kvp.Key] is Dictionary<string, object> targetDict && kvp.Value is Dictionary<string, object> sourceDict)
                {
                    result[kvp.Key] = DeepMerge(targetDict, sourceDict);
                }
                else
                {
                    result[kvp.Key] = kvp.Value;
                }
            }

            return result;
        }

        public static string GenerateRandomString(int length, string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
        {
            var random = new Random();
            return new string(Enumerable.Repeat(characters, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static byte[] GenerateRandomBytes(int length)
        {
            var bytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            return bytes;
        }
    }
}
