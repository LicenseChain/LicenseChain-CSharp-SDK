// LicenseChain C# SDK - Basic Usage Example
using System;
using System.Threading.Tasks;
using LicenseChain;
using LicenseChain.CSharp.SDK.Models;
using LicenseChain.CSharp.SDK.Exceptions;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("?? LicenseChain C# SDK - Basic Usage Example");
        Console.WriteLine("=" + new string('=', 50));
        
        // Initialize the client
        var client = new LicenseChainClient(new LicenseChainConfig
        {
            ApiKey = "your-api-key-here",
            AppName = "MyCSharpApp",
            Version = "1.0.0",
            Debug = true
        });
        
        // Connect to LicenseChain
        Console.WriteLine("\n Connecting to LicenseChain...");
        try
        {
            client.Connect();
            Console.WriteLine(" Connected to LicenseChain successfully!");
        }
        catch (LicenseChainException ex)
        {
            Console.WriteLine($" Failed to connect: {ex.Message}");
            return;
        }
        
        // Example 1: User Registration
        Console.WriteLine("\n Registering new user...");
        try
        {
            var user = client.Register("testuser", "password123", "test@example.com");
            Console.WriteLine(" User registered successfully!");
            Console.WriteLine($"Session ID: {user.SessionId}");
        }
        catch (LicenseChainException ex)
        {
            Console.WriteLine($" Registration failed: {ex.Message}");
        }
        
        // Example 2: License Validation
        Console.WriteLine("\n Validating license...");
        try
        {
            var license = client.ValidateLicense("LICENSE-KEY-HERE");
            Console.WriteLine(" License is valid!");
            Console.WriteLine($"License Key: {license.Key}");
            Console.WriteLine($"Status: {license.Status}");
        }
        catch (LicenseChainException ex)
        {
            Console.WriteLine($" License validation failed: {ex.Message}");
        }
        
        // Cleanup
        Console.WriteLine("\n Cleaning up...");
        client.Logout();
        client.Disconnect();
        Console.WriteLine(" Cleanup completed!");
    }
}
