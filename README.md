# LicenseChain C# SDK

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![C#](https://img.shields.io/badge/C%23-8.0+-blue.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![.NET](https://img.shields.io/badge/.NET-6.0+-green.svg)](https://dotnet.microsoft.com/)

Official C# SDK for LicenseChain - Secure license management for .NET applications.

## 🚀 Features

- **🔐 Secure Authentication** - User registration, login, and session management
- **📜 License Management** - Create, validate, update, and revoke licenses
- **🛡️ Hardware ID Validation** - Prevent license sharing and unauthorized access
- **🔔 Webhook Support** - Real-time license events and notifications
- **📊 Analytics Integration** - Track license usage and performance metrics
- **⚡ High Performance** - Optimized for production workloads
- **🔄 Async Operations** - Non-blocking HTTP requests and data processing
- **🛠️ Easy Integration** - Simple API with comprehensive documentation

## 📦 Installation

### Method 1: NuGet Package (Recommended)

```bash
# Install via Package Manager Console
Install-Package LicenseChain.SDK

# Or via .NET CLI
dotnet add package LicenseChain.SDK
```

### Method 2: PackageReference

Add to your `.csproj`:

```xml
<PackageReference Include="LicenseChain.SDK" Version="1.0.0" />
```

### Method 3: Manual Installation

1. Download the latest release from [GitHub Releases](https://github.com/LicenseChain/LicenseChain-CSharp-SDK/releases)
2. Add the DLL reference to your project
3. Install required dependencies

## 🚀 Quick Start

### Basic Setup

```csharp
using LicenseChain.SDK;

class Program
{
    static async Task Main(string[] args)
    {
        // Initialize the client
        var client = new LicenseChainClient(new LicenseChainConfig
        {
            ApiKey = "your-api-key",
            AppName = "your-app-name",
            Version = "1.0.0"
        });
        
        // Connect to LicenseChain
        try
        {
            await client.ConnectAsync();
            Console.WriteLine("Connected to LicenseChain successfully!");
        }
        catch (LicenseChainException ex)
        {
            Console.WriteLine($"Failed to connect: {ex.Message}");
            return;
        }
    }
}
```

### User Authentication

```csharp
// Register a new user
try
{
    var user = await client.RegisterAsync("username", "password", "email@example.com");
    Console.WriteLine("User registered successfully!");
    Console.WriteLine($"User ID: {user.Id}");
}
catch (LicenseChainException ex)
{
    Console.WriteLine($"Registration failed: {ex.Message}");
}

// Login existing user
try
{
    var user = await client.LoginAsync("username", "password");
    Console.WriteLine("User logged in successfully!");
    Console.WriteLine($"Session ID: {user.SessionId}");
}
catch (LicenseChainException ex)
{
    Console.WriteLine($"Login failed: {ex.Message}");
}
```

### License Management

```csharp
// Validate a license
try
{
    var license = await client.ValidateLicenseAsync("LICENSE-KEY-HERE");
    Console.WriteLine("License is valid!");
    Console.WriteLine($"License Key: {license.Key}");
    Console.WriteLine($"Status: {license.Status}");
    Console.WriteLine($"Expires: {license.Expires}");
    Console.WriteLine($"Features: {string.Join(", ", license.Features)}");
    Console.WriteLine($"User: {license.User}");
}
catch (LicenseChainException ex)
{
    Console.WriteLine($"License validation failed: {ex.Message}");
}

// Get user's licenses
try
{
    var licenses = await client.GetUserLicensesAsync();
    Console.WriteLine($"Found {licenses.Count} licenses:");
    for (int i = 0; i < licenses.Count; i++)
    {
        var license = licenses[i];
        Console.WriteLine($"  {i + 1}. {license.Key} - {license.Status} (Expires: {license.Expires})");
    }
}
catch (LicenseChainException ex)
{
    Console.WriteLine($"Failed to get licenses: {ex.Message}");
}
```

### Hardware ID Validation

```csharp
// Get hardware ID (automatically generated)
var hardwareId = client.GetHardwareId();
Console.WriteLine($"Hardware ID: {hardwareId}");

// Validate hardware ID with license
try
{
    var isValid = await client.ValidateHardwareIdAsync("LICENSE-KEY-HERE", hardwareId);
    if (isValid)
    {
        Console.WriteLine("Hardware ID is valid for this license!");
    }
    else
    {
        Console.WriteLine("Hardware ID is not valid for this license.");
    }
}
catch (LicenseChainException ex)
{
    Console.WriteLine($"Hardware ID validation failed: {ex.Message}");
}
```

### Webhook Integration

```csharp
// Set up webhook handler
client.SetWebhookHandler(async (eventName, data) =>
{
    Console.WriteLine($"Webhook received: {eventName}");
    
    switch (eventName)
    {
        case "license.created":
            Console.WriteLine($"New license created: {data["licenseKey"]}");
            break;
        case "license.updated":
            Console.WriteLine($"License updated: {data["licenseKey"]}");
            break;
        case "license.revoked":
            Console.WriteLine($"License revoked: {data["licenseKey"]}");
            break;
    }
});

// Start webhook listener
await client.StartWebhookListenerAsync();
```

## 📚 API Reference

### LicenseChainClient

#### Constructor

```csharp
var client = new LicenseChainClient(new LicenseChainConfig
{
    ApiKey = "your-api-key",
    AppName = "your-app-name",
    Version = "1.0.0",
    BaseUrl = "https://api.licensechain.app" // Optional
});
```

#### Methods

##### Connection Management

```csharp
// Connect to LicenseChain
await client.ConnectAsync();

// Disconnect from LicenseChain
await client.DisconnectAsync();

// Check connection status
bool isConnected = client.IsConnected;
```

##### User Authentication

```csharp
// Register a new user
var user = await client.RegisterAsync(username, password, email);

// Login existing user
var user = await client.LoginAsync(username, password);

// Logout current user
await client.LogoutAsync();

// Get current user info
var user = await client.GetCurrentUserAsync();
```

##### License Management

```csharp
// Validate a license
var license = await client.ValidateLicenseAsync(licenseKey);

// Get user's licenses
var licenses = await client.GetUserLicensesAsync();

// Create a new license
var license = await client.CreateLicenseAsync(userId, features, expires);

// Update a license
var license = await client.UpdateLicenseAsync(licenseKey, updates);

// Revoke a license
await client.RevokeLicenseAsync(licenseKey);

// Extend a license
var license = await client.ExtendLicenseAsync(licenseKey, days);
```

##### Hardware ID Management

```csharp
// Get hardware ID
string hardwareId = client.GetHardwareId();

// Validate hardware ID
bool isValid = await client.ValidateHardwareIdAsync(licenseKey, hardwareId);

// Bind hardware ID to license
await client.BindHardwareIdAsync(licenseKey, hardwareId);
```

##### Webhook Management

```csharp
// Set webhook handler
client.SetWebhookHandler(handler);

// Start webhook listener
await client.StartWebhookListenerAsync();

// Stop webhook listener
await client.StopWebhookListenerAsync();
```

##### Analytics

```csharp
// Track event
await client.TrackEventAsync(eventName, properties);

// Get analytics data
var analytics = await client.GetAnalyticsAsync(timeRange);
```

## 🔧 Configuration

### App Settings

Add to your `appsettings.json`:

```json
{
  "LicenseChain": {
    "ApiKey": "your-api-key",
    "AppName": "your-app-name",
    "Version": "1.0.0",
    "BaseUrl": "https://api.licensechain.app",
    "Timeout": 30,
    "Retries": 3,
    "Debug": false
  }
}
```

### Dependency Injection

```csharp
// In Startup.cs or Program.cs
services.AddLicenseChain(options =>
{
    options.ApiKey = Configuration["LicenseChain:ApiKey"];
    options.AppName = Configuration["LicenseChain:AppName"];
    options.Version = Configuration["LicenseChain:Version"];
});
```

### Environment Variables

Set these in your environment or through your build process:

```bash
# Required
export LICENSECHAIN_API_KEY=your-api-key
export LICENSECHAIN_APP_NAME=your-app-name
export LICENSECHAIN_APP_VERSION=1.0.0

# Optional
export LICENSECHAIN_BASE_URL=https://api.licensechain.app
export LICENSECHAIN_DEBUG=true
```

## 🛡️ Security Features

### Hardware ID Protection

The SDK automatically generates and manages hardware IDs to prevent license sharing:

```csharp
// Hardware ID is automatically generated and stored
var hardwareId = client.GetHardwareId();

// Validate against license
var isValid = await client.ValidateHardwareIdAsync(licenseKey, hardwareId);
```

### Secure Communication

- All API requests use HTTPS
- API keys are securely stored and transmitted
- Session tokens are automatically managed
- Webhook signatures are verified

### License Validation

- Real-time license validation
- Hardware ID binding
- Expiration checking
- Feature-based access control

## 📊 Analytics and Monitoring

### Event Tracking

```csharp
// Track custom events
await client.TrackEventAsync("app.started", new Dictionary<string, object>
{
    ["level"] = 1,
    ["playerCount"] = 10
});

// Track license events
await client.TrackEventAsync("license.validated", new Dictionary<string, object>
{
    ["licenseKey"] = "LICENSE-KEY",
    ["features"] = "premium,unlimited"
});
```

### Performance Monitoring

```csharp
// Get performance metrics
var metrics = await client.GetPerformanceMetricsAsync();
Console.WriteLine($"API Response Time: {metrics.AverageResponseTime}ms");
Console.WriteLine($"Success Rate: {metrics.SuccessRate:P}");
Console.WriteLine($"Error Count: {metrics.ErrorCount}");
```

## 🔄 Error Handling

### Custom Exception Types

```csharp
try
{
    var license = await client.ValidateLicenseAsync("invalid-key");
}
catch (InvalidLicenseException ex)
{
    Console.WriteLine("License key is invalid");
}
catch (ExpiredLicenseException ex)
{
    Console.WriteLine("License has expired");
}
catch (NetworkException ex)
{
    Console.WriteLine("Network connection failed");
}
catch (LicenseChainException ex)
{
    Console.WriteLine($"LicenseChain error: {ex.Message}");
}
```

### Retry Logic

```csharp
// Automatic retry for network errors
var client = new LicenseChainClient(new LicenseChainConfig
{
    ApiKey = "your-api-key",
    AppName = "your-app-name",
    Version = "1.0.0",
    Retries = 3, // Retry up to 3 times
    RetryDelay = TimeSpan.FromSeconds(1) // Wait 1 second between retries
});
```

## 🧪 Testing

### Unit Tests

```bash
# Run tests
dotnet test
```

### Integration Tests

```bash
# Test with real API
dotnet test --filter Category=Integration
```

## 📝 Examples

See the `examples/` directory for complete examples:

- `basic_usage.cs` - Basic SDK usage
- `advanced_features.cs` - Advanced features and configuration
- `webhook_integration.cs` - Webhook handling

## 🤝 Contributing

We welcome contributions! Please see our [Contributing Guide](CONTRIBUTING.md) for details.

### Development Setup

1. Clone the repository
2. Install .NET 6.0 or later
3. Build: `dotnet build`
4. Test: `dotnet test`

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🆘 Support

- **Documentation**: [https://docs.licensechain.app/csharp](https://docs.licensechain.app/csharp)
- **Issues**: [GitHub Issues](https://github.com/LicenseChain/LicenseChain-CSharp-SDK/issues)
- **Discord**: [LicenseChain Discord](https://discord.gg/licensechain)
- **Email**: support@licensechain.app

## 🔗 Related Projects

- [LicenseChain JavaScript SDK](https://github.com/LicenseChain/LicenseChain-JavaScript-SDK)
- [LicenseChain Python SDK](https://github.com/LicenseChain/LicenseChain-Python-SDK)
- [LicenseChain Node.js SDK](https://github.com/LicenseChain/LicenseChain-NodeJS-SDK)
- [LicenseChain Customer Panel](https://github.com/LicenseChain/LicenseChain-Customer-Panel)

---

**Made with ❤️ for the .NET community**
