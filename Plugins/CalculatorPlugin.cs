using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace ToolCallingAssistant.Plugins;

public class CalculatorPlugin
{
    [KernelFunction("add_numbers")]
    [Description("Adds two numbers.")]
    public double AddNumbers(
        [Description("First number")] double a,
        [Description("Second number")] double b)
    {
        Console.WriteLine(
            $"[Tool] add_numbers({a}, {b})");

        return a + b;
    }

    [KernelFunction("multiply_numbers")]
    [Description("Multiplies two numbers.")]
    public double MultiplyNumbers(
        [Description("First number")] double a,
        [Description("Second number")] double b)
    {
        Console.WriteLine(
            $"[Tool] multiply_numbers({a}, {b})");

        return a * b;
    }
}

