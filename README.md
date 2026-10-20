# C# Tool Calling Assistant

A .NET console application that demonstrates tool calling with Microsoft Semantic Kernel and a local Ollama model.

This project is based on the pattern of an AI assistant that can call built-in functions to:

- perform arithmetic calculations
- look up product pricing information
- respond to user questions with tool-augmented reasoning

## Features

- Semantic Kernel chat completion using OpenAI-compatible Ollama endpoint
- Automatic function calling enabled via `FunctionChoiceBehavior.Auto()`
- Built-in `CalculatorPlugin` for addition and multiplication
- Built-in `ProductPlugin` for product lookup and listing
- Interactive console experience for testing agent behavior

## Project structure

```text
Csharp-ToolCallingAssistant/
├── Program.cs
├── ToolCallingAssistant.csproj
├── ToolCallingAssistant.slnx
├── Plugins/
│   ├── CalculatorPlugin.cs
│   └── ProductPlugin.cs
├── .gitignore
├── .gitattributes
└── README.md
```

## Prerequisites

- .NET SDK 8 or later
- Ollama installed and running locally
- A model available in Ollama, such as `qwen3.5:9b`

## Setup

1. Install and start Ollama.
2. Pull the model used by the app:

```bash
ollama pull qwen3.5:9b
```

3. Restore NuGet packages:

```bash
dotnet restore
```

4. Run the app:

```bash
dotnet run
```

## How it works

The application creates a `Kernel`, registers tool plugins, and configures automatic function calling:

- `CalculatorPlugin`
  - `add_numbers(a, b)`
  - `multiply_numbers(a, b)`
- `ProductPlugin`
  - `get_product_price(productName)`
  - `list_products()`

The console menu allows you to:

1. Ask the AI assistant a question
2. Manually test the calculator
3. Clear the conversation
4. Exit the program

## Example questions

- "What is 25 * 12?"
- "What is the price of a laptop?"
- "List all products and prices."

## Notes

The app is configured to connect to Ollama at:

```text
http://localhost:11434/v1
```

and uses the API key value `ollama` as expected by the local OpenAI-compatible endpoint.

## License

This project does not currently include a license file. If you are publishing or sharing the project publicly, consider adding an appropriate open-source license.
