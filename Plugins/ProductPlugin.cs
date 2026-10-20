using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace ToolCallingAssistant.Plugins;

public class ProductPlugin
{
    private readonly Dictionary<string, decimal> _products =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["Laptop"] = 3500m,
            ["Monitor"] = 850m,
            ["Keyboard"] = 120m,
            ["Mouse"] = 80m
        };

    [KernelFunction("get_product_price")]
    [Description(
        "Gets the selling price of a product in RM. " +
        "Use this function when the user asks about " +
        "a product price.")]
    public string GetProductPrice(
        [Description("Product name")] string productName)
    {
        Console.WriteLine(
            $"[Tool] get_product_price({productName})");

        if (string.IsNullOrWhiteSpace(productName))
            return "Product name is required.";

        if (_products.TryGetValue(
            productName.Trim(), out decimal price))
        {
            return $"{productName}: RM{price:F2}";
        }

        return $"Product not found: {productName}";
    }

    [KernelFunction("list_products")]
    [Description("Lists available products and prices.")]
    public string ListProducts()
    {
        Console.WriteLine("[Tool] list_products()");

        return string.Join(
            Environment.NewLine,
            _products.Select(p =>
                $"{p.Key}: RM{p.Value:F2}"));
    }
}

