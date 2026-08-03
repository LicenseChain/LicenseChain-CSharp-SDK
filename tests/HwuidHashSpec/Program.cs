using System.Reflection;
using System.Text.RegularExpressions;
using LicenseChain;

var method = typeof(LicenseChainClient).GetMethod("GenerateDefaultHwuid", BindingFlags.NonPublic | BindingFlags.Static);
if (method is null) throw new Exception("GenerateDefaultHwuid method not found");

var h1 = method.Invoke(null, null)?.ToString() ?? string.Empty;
var h2 = method.Invoke(null, null)?.ToString() ?? string.Empty;

if (h1 != h2) throw new Exception("default hwuid must be deterministic");
if (!Regex.IsMatch(h1, "^[a-f0-9]{64}$")) throw new Exception("default hwuid must be lowercase sha256 hex");

Console.WriteLine("HWUID hash spec: ok");
