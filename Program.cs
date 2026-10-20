using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using ToolCallingAssistant.Plugins;

Console.Title = "C# Tool Calling Assistant";

// Increase timeout for local inference.
using var httpClient = new HttpClient
{
    Timeout = TimeSpan.FromMinutes(10)
};

// Configure the Kernel.
var builder = Kernel.CreateBuilder();

builder.AddOpenAIChatCompletion(
    modelId: "qwen3.5:9b",
    endpoint: new Uri("http://localhost:11434/v1"),
    apiKey: "ollama",
    httpClient: httpClient);

Kernel kernel = builder.Build();

// Register read-only plugins.
kernel.Plugins.AddFromType<CalculatorPlugin>(
    "Calculator");

kernel.Plugins.AddFromType<ProductPlugin>(
    "Product");

// Get the chat completion service.
IChatCompletionService chatService =
    kernel.GetRequiredService<IChatCompletionService>();

// Enable automatic function calling.
var settings = new OpenAIPromptExecutionSettings
{
    FunctionChoiceBehavior =
        FunctionChoiceBehavior.Auto(),

    Temperature = 0.2,
    MaxTokens = 500
};

// Create conversation history.
var history = new ChatHistory();

history.AddSystemMessage(
    "You are a helpful business assistant. " +
    "Use available tools for calculations and product data. " +
    "Do not invent product prices. " +
    "If a product is unavailable, say so. " +
    "Keep responses concise.");

Console.WriteLine("==================================");
Console.WriteLine("       TOOL CALLING ASSISTANT");
Console.WriteLine("       Visual Studio 2026");
Console.WriteLine("       Semantic Kernel + Ollama");
Console.WriteLine("==================================");

while (true)
{
    Console.WriteLine();
    Console.WriteLine("1. Ask the AI Agent");
    Console.WriteLine("2. Test Calculator Manually");
    Console.WriteLine("3. Clear Conversation");
    Console.WriteLine("0. Exit");
    Console.WriteLine();

    Console.Write("Select option: ");
    string? choice = Console.ReadLine();

    if (choice == "0")
        break;

    try
    {
        switch (choice)
        {
            case "1":
                Console.Write("Your question: ");
                string question = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(question))
                    break;

                history.AddUserMessage(question);

                Console.WriteLine();
                Console.WriteLine(
                    "Qwen is processing your request...");

                try
                {
                    var response =
                        await chatService
                            .GetChatMessageContentAsync(
                                history,
                                executionSettings: settings,
                                kernel: kernel);

                    Console.WriteLine();
                    Console.WriteLine("Assistant:");
                    Console.WriteLine(
                        response.Content ??
                        "(No text response returned)");

                    history.AddAssistantMessage(
                        response.Content ?? "");
                }
                catch
                {
                    // Discard failed-turn history because
                    // automatic tool calls may leave
                    // partial messages in the history.
                    history = new ChatHistory();

                    history.AddSystemMessage(
                        "You are a helpful business assistant. " +
                        "Use available tools for calculations " +
                        "and product data. " +
                        "Do not invent product prices.");

                    throw;
                }

                break;

            case "2":
                var result = await kernel.InvokeAsync(
                    "Calculator",
                    "multiply_numbers",
                    new KernelArguments
                    {
                        ["a"] = 25.0,
                        ["b"] = 12.0
                    });

                Console.WriteLine(
                    $"Manual result: {result}");

                break;

            case "3":
                history = new ChatHistory();

                history.AddSystemMessage(
                    "You are a helpful business assistant. " +
                    "Use available tools for calculations " +
                    "and product data. " +
                    "Do not invent product prices.");

                Console.WriteLine(
                    "Conversation cleared.");

                break;

            default:
                Console.WriteLine(
                    "Invalid selection.");
                break;
        }
    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine(
            "Ollama connection error: " + ex.Message);
    }
    catch (OperationCanceledException)
    {
        Console.WriteLine(
            "The AI request timed out or was cancelled.");
    }
    catch (Exception ex)
    {
        Console.WriteLine(
            "Error: " + ex.Message);
    }
}

Console.WriteLine("Goodbye!");
