// LicenseChain C# SDK - Basic Usage Example
using System;
using System.Threading.Tasks;
using LicenseChain;
using LicenseChain.CSharp.SDK.Models;
using LicenseChain.CSharp.SDK.Exceptions;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("🚀 LicenseChain C# SDK - Basic Usage Example");
        Console.WriteLine("=" + new string('=', 50));
        
        // Initialize the client with your API key
        var client = new LicenseChainClient("your-api-key-here");
        
        try
        {
            // Example 1: User Registration
            Console.WriteLine("\n📝 Registering new user...");
            var registerRequest = new UserRegistrationRequest
            {
                Email = "test@example.com",
                Password = "password123",
                Name = "Test User"
            };
            var user = await client.RegisterUserAsync(registerRequest);
            Console.WriteLine($"✅ User registered successfully! ID: {user.Id}");
            
            // Example 2: User Login
            Console.WriteLine("\n🔐 Logging in...");
            var loginRequest = new LoginRequest
            {
                Email = "test@example.com",
                Password = "password123"
            };
            var loginResponse = await client.LoginAsync(loginRequest);
            Console.WriteLine($"✅ Login successful! Token: {loginResponse.Token.Substring(0, 20)}...");
            
            // Example 3: Get Current User
            Console.WriteLine("\n👤 Getting current user...");
            var currentUser = await client.GetUserProfileAsync();
            Console.WriteLine($"✅ Current user: {currentUser.Name} ({currentUser.Email})");
            
            // Example 4: Create Application
            Console.WriteLine("\n📱 Creating application...");
            var appRequest = new ApplicationCreateRequest
            {
                Name = "My Test App",
                Description = "A test application"
            };
            var app = await client.CreateApplicationAsync(appRequest);
            Console.WriteLine($"✅ Application created! ID: {app.Id}");
            
            // Example 5: License Validation
            Console.WriteLine("\n🔑 Validating license...");
            var validationResult = await client.ValidateLicenseAsync("LICENSE-KEY-HERE");
            Console.WriteLine($"✅ License validation: {(validationResult.Valid ? "Valid" : "Invalid")}");
            
            // Example 6: Logout
            Console.WriteLine("\n🚪 Logging out...");
            await client.LogoutAsync();
            Console.WriteLine("✅ Logged out successfully!");
        }
        catch (LicenseChainException ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message} (Code: {ex.ErrorCode}, Status: {ex.StatusCode})");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Unexpected error: {ex.Message}");
        }
        finally
        {
            client.Dispose();
            Console.WriteLine("\n✅ Cleanup completed!");
        }
    }
}
