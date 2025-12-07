using System;

namespace LicenseChain.CSharp.SDK.Exceptions
{
    public class LicenseChainException : Exception
    {
        public string ErrorCode { get; }
        public int StatusCode { get; }

        public LicenseChainException(string message) : base(message)
        {
            ErrorCode = "UNKNOWN_ERROR";
            StatusCode = 500;
        }

        public LicenseChainException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = "UNKNOWN_ERROR";
            StatusCode = 500;
        }

        public LicenseChainException(string errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
            StatusCode = 500;
        }

        public LicenseChainException(string errorCode, string message, int statusCode) : base(message)
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
        }

        public LicenseChainException(string errorCode, string message, int statusCode, Exception innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
        }
    }

    public class NetworkException : LicenseChainException
    {
        public NetworkException(string message) : base("NETWORK_ERROR", message, 0)
        {
        }

        public NetworkException(string message, Exception innerException) : base("NETWORK_ERROR", message, 0, innerException)
        {
        }
    }

    public class ValidationException : LicenseChainException
    {
        public ValidationException(string message) : base("VALIDATION_ERROR", message, 400)
        {
        }

        public ValidationException(string message, Exception innerException) : base("VALIDATION_ERROR", message, 400, innerException)
        {
        }
    }

    public class AuthenticationException : LicenseChainException
    {
        public AuthenticationException(string message) : base("AUTHENTICATION_ERROR", message, 401)
        {
        }

        public AuthenticationException(string message, Exception innerException) : base("AUTHENTICATION_ERROR", message, 401, innerException)
        {
        }
    }

    public class NotFoundException : LicenseChainException
    {
        public NotFoundException(string message) : base("NOT_FOUND_ERROR", message, 404)
        {
        }

        public NotFoundException(string message, Exception innerException) : base("NOT_FOUND_ERROR", message, 404, innerException)
        {
        }
    }

    public class RateLimitException : LicenseChainException
    {
        public RateLimitException(string message) : base("RATE_LIMIT_ERROR", message, 429)
        {
        }

        public RateLimitException(string message, Exception innerException) : base("RATE_LIMIT_ERROR", message, 429, innerException)
        {
        }
    }

    public class ServerException : LicenseChainException
    {
        public ServerException(string message) : base("SERVER_ERROR", message, 500)
        {
        }

        public ServerException(string message, Exception innerException) : base("SERVER_ERROR", message, 500, innerException)
        {
        }
    }

    public class ConfigurationException : LicenseChainException
    {
        public ConfigurationException(string message) : base("CONFIGURATION_ERROR", message, 500)
        {
        }

        public ConfigurationException(string message, Exception innerException) : base("CONFIGURATION_ERROR", message, 500, innerException)
        {
        }
    }
}
