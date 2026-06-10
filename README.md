# provider-check

A 60-second sanity check that your **.NET AI development environment** actually works - against a free **local** model (Ollama) and the free **cloud** tier (GitHub Models), using the exact same C# code for both.

If you are about to start building AI features into a .NET app and want to confirm "base camp is solid" before writing real code, clone this, run it, and you will know in under a minute.

```
== provider-check: ollama ==

Endpoint: http://localhost:11434/
Model:    llama3.2

You: In one sentence, what is an API?
AI:  An API is a set of rules that lets one program request data or actions from another.

Success. Your 'ollama' setup works - you are ready to build.
```

---

## Why this exists

This is the companion starter for **Chapter 2** of the Coding Droplets course
[**AI-Powered APIs in .NET**](https://aiapis.codingdroplets.com). The whole idea it
demonstrates: you write your code against `Microsoft.Extensions.AI`'s `IChatClient`
interface, and the provider behind it - local Ollama, GitHub Models, OpenAI, Azure
OpenAI - becomes a one-line choice. Switching providers does not change your app.

> **Learn the whole stack:** chat, structured outputs, RAG, tool calling, agents, MCP,
> security, evaluation, observability, and deployment - all in .NET, built one runnable
> project at a time. **[AI-Powered APIs in .NET →](https://aiapis.codingdroplets.com)**

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- For the local path: [Ollama](https://ollama.com/) running, with a model pulled:
  ```
  ollama pull llama3.2
  ```
- For the cloud path: a [GitHub Models](https://github.com/marketplace/models) token
  (free tier) with the `models:read` permission.

## Quick start

Clone and run the local check (the default):

```
git clone https://github.com/codingdroplets/provider-check.git
cd provider-check
dotnet run -- ollama
```

Check the cloud provider (GitHub Models free tier):

```
# PowerShell
$env:GITHUB_TOKEN = "your-token-here"
dotnet run -- github
```

```
# bash
export GITHUB_TOKEN="your-token-here"
dotnet run -- github
```

## Configuration (optional overrides)

The defaults work out of the box. To point the checker at a different endpoint or model,
set environment variables before running:

- `OLLAMA_ENDPOINT` - default `http://localhost:11434/`
- `OLLAMA_MODEL` - default `llama3.2`
- `GITHUB_TOKEN` - required for the `github` provider (`models:read` permission)
- `GITHUB_MODEL` - default `openai/gpt-4o-mini`
- `GITHUB_MODELS_ENDPOINT` - default `https://models.github.ai/inference`

## How it works

| File | Job |
|------|-----|
| `Program.cs` | Picks the provider from the command line, runs the check, prints a clear verdict. |
| `ChatClientFactory.cs` | The only provider-specific code - builds an `IChatClient` for Ollama or GitHub Models. |
| `ChatRunner.cs` | Sends one question and prints the answer. Provider-agnostic - it only sees `IChatClient`. |

That separation is the point: 95% of the code never knows which model answered.

## Built with

- [`Microsoft.Extensions.AI`](https://learn.microsoft.com/en-us/dotnet/ai/microsoft-extensions-ai) - the provider-agnostic `IChatClient` abstraction
- [`OllamaSharp`](https://www.nuget.org/packages/OllamaSharp) - `IChatClient` over a local Ollama server
- [`Microsoft.Extensions.AI.OpenAI`](https://www.nuget.org/packages/Microsoft.Extensions.AI.OpenAI) - `IChatClient` over OpenAI-compatible endpoints (GitHub Models, OpenAI, Azure OpenAI)

## Learn more

- **Course:** [AI-Powered APIs in .NET](https://aiapis.codingdroplets.com) - the full, structured guide this starter belongs to
- **Blog:** [codingdroplets.com](https://codingdroplets.com) - keeping up with the fast-moving .NET AI stack
- **More free starters:** [github.com/codingdroplets](https://github.com/codingdroplets)

## License

[MIT](LICENSE) - free to use, modify, and build on.
