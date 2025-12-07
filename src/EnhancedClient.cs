using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Linq;

namespace LicenseChain
{
    /// <summary>
    /// Enhanced LicenseChain client with advanced features
    /// Comprehensive license management with enhanced functionality
    /// </summary>
    public class EnhancedClient : LicenseChainClient
    {
        private string _sessionId;
        private User _userData;
        private bool _initialized;

        public EnhancedClient(string appName, string ownerId, string appSecret, string baseUrl = "https://api.licensechain.app", int timeout = 30, int retries = 3)
            : base(appName, ownerId, appSecret, baseUrl, timeout, retries)
        {
        }

        /// <summary>
        /// Initialize the client and establish connection
        /// </summary>
        public async Task<bool> InitAsync()
        {
            if (_initialized)
                return true;

            try
            {
                var requestData = new
                {
                    type = "init",
                    ver = "1.0",
                    hash = GenerateHash(),
                    enckey = GenerateEncryptionKey(),
                    name = AppName,
                    ownerid = OwnerId
                };

                var response = await MakeRequestAsync("init", requestData);

                if (response.Success)
                {
                    _sessionId = response.SessionId;
                    _initialized = true;
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new LicenseChainException($"Initialization failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Login with license key only (advanced pattern)
        /// </summary>
        public async Task<LicenseLoginResponse> LicenseLoginAsync(string licenseKey)
        {
            EnsureInitialized();

            var requestData = new
            {
                type = "license",
                key = licenseKey,
                hwid = GetHardwareId()
            };

            var response = await MakeRequestAsync("license", requestData);

            if (response.Success)
            {
                _userData = response.User;
                return response;
            }
            else
            {
                throw new LicenseChainException(response.Message ?? "License login failed");
            }
        }

        /// <summary>
        /// Check if user is logged in
        /// </summary>
        public bool IsLoggedIn()
        {
            return _userData != null;
        }

        /// <summary>
        /// Get current user data
        /// </summary>
        public User GetUserData()
        {
            return _userData;
        }

        /// <summary>
        /// Get user's subscription information
        /// </summary>
        public List<string> GetSubscription()
        {
            if (!IsLoggedIn())
                return null;
            return _userData?.Subscriptions;
        }

        /// <summary>
        /// Get user's variables
        /// </summary>
        public Dictionary<string, string> GetVariables()
        {
            if (!IsLoggedIn())
                return null;
            return _userData?.Variables;
        }

        /// <summary>
        /// Get user's data
        /// </summary>
        public Dictionary<string, object> GetData()
        {
            if (!IsLoggedIn())
                return null;
            return _userData?.Data;
        }

        /// <summary>
        /// Set user variable
        /// </summary>
        public async Task<bool> SetVarAsync(string varName, string data)
        {
            EnsureLoggedIn();

            var requestData = new
            {
                type = "setvar",
                var = varName,
                data = data,
                sessionid = _sessionId
            };

            var response = await MakeRequestAsync("setvar", requestData);
            return response.Success;
        }

        /// <summary>
        /// Get user variable
        /// </summary>
        public async Task<string> GetVarAsync(string varName)
        {
            EnsureLoggedIn();

            var requestData = new
            {
                type = "getvar",
                var = varName,
                sessionid = _sessionId
            };

            var response = await MakeRequestAsync("getvar", requestData);
            return response.Success ? response.Data : null;
        }

        /// <summary>
        /// Log message to LicenseChain
        /// </summary>
        public async Task<bool> LogMessageAsync(string message)
        {
            EnsureLoggedIn();

            var requestData = new
            {
                type = "log",
                pcuser = GetPcUser(),
                message = message,
                sessionid = _sessionId
            };

            var response = await MakeRequestAsync("log", requestData);
            return response.Success;
        }

        /// <summary>
        /// Download file from LicenseChain
        /// </summary>
        public async Task<string> DownloadFileAsync(string fileId)
        {
            EnsureLoggedIn();

            var requestData = new
            {
                type = "file",
                fileid = fileId,
                sessionid = _sessionId
            };

            var response = await MakeRequestAsync("file", requestData);
            return response.Success ? response.Contents : null;
        }

        /// <summary>
        /// Get application statistics
        /// </summary>
        public async Task<Dictionary<string, object>> GetAppStatsAsync()
        {
            EnsureInitialized();

            var requestData = new
            {
                type = "app",
                sessionid = _sessionId
            };

            var response = await MakeRequestAsync("app", requestData);
            return response.Success ? response.Data : null;
        }

        /// <summary>
        /// Get online users
        /// </summary>
        public async Task<List<User>> GetOnlineUsersAsync()
        {
            EnsureLoggedIn();

            var requestData = new
            {
                type = "online",
                sessionid = _sessionId
            };

            var response = await MakeRequestAsync("online", requestData);
            return response.Success ? response.Users : null;
        }

        /// <summary>
        /// Get chat messages
        /// </summary>
        public async Task<List<ChatMessage>> ChatGetAsync(string channel = "general")
        {
            EnsureLoggedIn();

            var requestData = new
            {
                type = "chatget",
                channel = channel,
                sessionid = _sessionId
            };

            var response = await MakeRequestAsync("chatget", requestData);
            return response.Success ? response.Messages : null;
        }

        /// <summary>
        /// Send chat message
        /// </summary>
        public async Task<bool> ChatSendAsync(string message, string channel = "general")
        {
            EnsureLoggedIn();

            var requestData = new
            {
                type = "chatsend",
                message = message,
                channel = channel,
                sessionid = _sessionId
            };

            var response = await MakeRequestAsync("chatsend", requestData);
            return response.Success;
        }

        /// <summary>
        /// Ban user
        /// </summary>
        public async Task<bool> BanUserAsync(string username)
        {
            EnsureLoggedIn();

            var requestData = new
            {
                type = "ban",
                user = username,
                sessionid = _sessionId
            };

            var response = await MakeRequestAsync("ban", requestData);
            return response.Success;
        }

        /// <summary>
        /// Unban user
        /// </summary>
        public async Task<bool> UnbanUserAsync(string username)
        {
            EnsureLoggedIn();

            var requestData = new
            {
                type = "unban",
                user = username,
                sessionid = _sessionId
            };

            var response = await MakeRequestAsync("unban", requestData);
            return response.Success;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        public async Task<List<User>> GetAllUsersAsync()
        {
            EnsureLoggedIn();

            var requestData = new
            {
                type = "allusers",
                sessionid = _sessionId
            };

            var response = await MakeRequestAsync("allusers", requestData);
            return response.Success ? response.Users : null;
        }

        /// <summary>
        /// Get user by username
        /// </summary>
        public async Task<User> GetUserAsync(string username)
        {
            EnsureLoggedIn();

            var requestData = new
            {
                type = "getuser",
                user = username,
                sessionid = _sessionId
            };

            var response = await MakeRequestAsync("getuser", requestData);
            return response.Success ? response.User : null;
        }

        /// <summary>
        /// Update user data
        /// </summary>
        public async Task<bool> UpdateUserAsync(string username, Dictionary<string, object> data)
        {
            EnsureLoggedIn();

            var requestData = new
            {
                type = "edituser",
                user = username,
                data = data,
                sessionid = _sessionId
            };

            var response = await MakeRequestAsync("edituser", requestData);
            return response.Success;
        }

        /// <summary>
        /// Delete user
        /// </summary>
        public async Task<bool> DeleteUserAsync(string username)
        {
            EnsureLoggedIn();

            var requestData = new
            {
                type = "deleteuser",
                user = username,
                sessionid = _sessionId
            };

            var response = await MakeRequestAsync("deleteuser", requestData);
            return response.Success;
        }

        /// <summary>
        /// Get webhook data
        /// </summary>
        public async Task<Dictionary<string, object>> GetWebhookAsync()
        {
            EnsureLoggedIn();

            var requestData = new
            {
                type = "webhook",
                sessionid = _sessionId
            };

            var response = await MakeRequestAsync("webhook", requestData);
            return response.Success ? response.Data : null;
        }

        /// <summary>
        /// Verify webhook signature
        /// </summary>
        public bool VerifyWebhook(string payload, string signature)
        {
            var expectedSignature = GenerateWebhookSignature(payload);
            return expectedSignature == signature;
        }

        /// <summary>
        /// Parse webhook payload
        /// </summary>
        public Dictionary<string, object> ParseWebhook(string payload, string signature)
        {
            if (!VerifyWebhook(payload, signature))
                return null;

            try
            {
                return JsonSerializer.Deserialize<Dictionary<string, object>>(payload);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Logout the current user
        /// </summary>
        public async Task<bool> LogoutAsync()
        {
            if (!IsLoggedIn())
                return true;

            try
            {
                var requestData = new
                {
                    type = "logout",
                    sessionid = _sessionId
                };

                var response = await MakeRequestAsync("logout", requestData);

                if (response.Success)
                {
                    _userData = null;
                    _sessionId = null;
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private void EnsureInitialized()
        {
            if (!_initialized)
                throw new LicenseChainException("Client not initialized. Call InitAsync() first.");
        }

        private void EnsureLoggedIn()
        {
            EnsureInitialized();
            if (!IsLoggedIn())
                throw new LicenseChainException("User not logged in");
        }

        private string GenerateHash()
        {
            var data = $"{AppName}{OwnerId}{AppSecret}";
            return ComputeSha256Hash(data);
        }

        private string GenerateEncryptionKey()
        {
            return GenerateRandomString(32);
        }

        private string GetHardwareId()
        {
            try
            {
                var machineName = Environment.MachineName;
                var osVersion = Environment.OSVersion.ToString();
                var processorCount = Environment.ProcessorCount.ToString();
                return $"{machineName}-{osVersion}-{processorCount}";
            }
            catch
            {
                return $"unknown-hwid-{GenerateRandomString(8)}";
            }
        }

        private string GetPcUser()
        {
            try
            {
                return Environment.UserName ?? "unknown";
            }
            catch
            {
                return "unknown";
            }
        }

        private string GenerateWebhookSignature(string payload)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(AppSecret)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
                return $"sha256={Convert.ToHexString(hash).ToLower()}";
            }
        }

        private string ComputeSha256Hash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToHexString(bytes).ToLower();
            }
        }

        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }

    /// <summary>
    /// Chat message model
    /// </summary>
    public class ChatMessage
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }
        public string Channel { get; set; }
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// License login response model
    /// </summary>
    public class LicenseLoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public User User { get; set; }
        public string SessionId { get; set; }
        public Dictionary<string, object> Data { get; set; }
        public List<string> Users { get; set; }
        public List<ChatMessage> Messages { get; set; }
        public string Contents { get; set; }
    }
}
