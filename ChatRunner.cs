using Microsoft.Extensions.AI;

namespace CodingDroplets.ProviderCheck;

/// <summary>
/// Sends one question and prints the answer. This method does not know or care which
/// provider is behind <see cref="IChatClient"/> - the exact same code runs against
/// Ollama and GitHub Models. That is the core lesson the checker proves.
/// </summary>
public static class ChatRunner
{
    public static async Task RunAsync(IChatClient chatClient)
    {
        const string question = "In one sentence, what is an API?";

        Console.WriteLine($"You: {question}");

        ChatResponse response = await chatClient.GetResponseAsync(question);

        Console.WriteLine($"AI:  {response.Text}");
    }
}
