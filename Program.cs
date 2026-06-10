using CodingDroplets.ProviderCheck;
using Microsoft.Extensions.AI;

// provider-check: a 60-second sanity check that your .NET AI dev environment works.
//
// Usage:
//   dotnet run -- ollama    (local, free - the default)
//   dotnet run -- github    (GitHub Models free tier, needs GITHUB_TOKEN)
//
// The whole point: the same IChatClient code runs against either provider.
string provider = args.Length > 0 ? args[0].Trim().ToLowerInvariant() : "ollama";

Console.WriteLine($"== provider-check: {provider} ==");
Console.WriteLine();

try
{
    // The ONLY provider-specific line. Everything after it is identical.
    using IChatClient chatClient = ChatClientFactory.Create(provider);

    await ChatRunner.RunAsync(chatClient);

    Console.WriteLine();
    Console.WriteLine($"Success. Your '{provider}' setup works - you are ready to build.");
}
catch (Exception ex)
{
    // Friendly, actionable error instead of a stack trace.
    Console.WriteLine();
    Console.WriteLine($"Setup check failed for '{provider}':");
    Console.WriteLine($"  {ex.Message}");
    Environment.ExitCode = 1;
}
