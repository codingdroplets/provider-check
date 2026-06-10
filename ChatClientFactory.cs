using System.ClientModel;
using Microsoft.Extensions.AI;
using OllamaSharp;
using OpenAI;

namespace CodingDroplets.ProviderCheck;

/// <summary>
/// Builds an <see cref="IChatClient"/> for a named provider. This is the one place
/// that knows which provider is which - the rest of the app only sees IChatClient.
/// Endpoints and models can be overridden with environment variables so this works
/// as a generic checker, not just for the defaults.
/// </summary>
public static class ChatClientFactory
{
    public static IChatClient Create(string provider) => provider switch
    {
        "ollama" => CreateOllama(),
        "github" => CreateGitHubModels(),
        _ => throw new ArgumentException(
            $"Unknown provider '{provider}'. Use 'ollama' or 'github'.")
    };

    // Local and free. Requires Ollama running and the model pulled (e.g. `ollama pull llama3.2`).
    // Override with OLLAMA_ENDPOINT / OLLAMA_MODEL if your setup differs.
    private static IChatClient CreateOllama()
    {
        string endpoint = Environment.GetEnvironmentVariable("OLLAMA_ENDPOINT") ?? "http://localhost:11434/";
        string model = Environment.GetEnvironmentVariable("OLLAMA_MODEL") ?? "llama3.2";

        Console.WriteLine($"Endpoint: {endpoint}");
        Console.WriteLine($"Model:    {model}");
        Console.WriteLine();

        // OllamaApiClient implements IChatClient directly.
        return new OllamaApiClient(new Uri(endpoint), model);
    }

    // Cloud, free tier. Requires a GitHub token (with the 'models:read' permission)
    // in the GITHUB_TOKEN environment variable. Override the model with GITHUB_MODEL.
    private static IChatClient CreateGitHubModels()
    {
        string? token = Environment.GetEnvironmentVariable("GITHUB_TOKEN");
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new InvalidOperationException(
                "GITHUB_TOKEN is not set.\n" +
                "  Create a GitHub token with the 'models:read' permission, then set it.\n" +
                "  PowerShell:  $env:GITHUB_TOKEN = \"your-token-here\"\n" +
                "  bash:        export GITHUB_TOKEN=\"your-token-here\"");
        }

        string endpoint = Environment.GetEnvironmentVariable("GITHUB_MODELS_ENDPOINT") ?? "https://models.github.ai/inference";
        string model = Environment.GetEnvironmentVariable("GITHUB_MODEL") ?? "openai/gpt-4o-mini";

        Console.WriteLine($"Endpoint: {endpoint}");
        Console.WriteLine($"Model:    {model}");
        Console.WriteLine();

        // GitHub Models speaks the OpenAI API, so we use the OpenAI SDK pointed at the
        // GitHub endpoint, then adapt it to IChatClient.
        OpenAIClient openAIClient = new(
            new ApiKeyCredential(token),
            new OpenAIClientOptions { Endpoint = new Uri(endpoint) });

        return openAIClient.GetChatClient(model).AsIChatClient();
    }
}
