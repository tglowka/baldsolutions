using System.Text;

namespace DragonBook;

public static class OutputBuilder
{
    private static readonly StringBuilder Sb = new();

    public static void Emit(string message)
        => Sb.AppendLine(message);
    
    public static void EmitLabel(int labelNo)
        => Sb.Append($"L{labelNo}:");

    public static string Build()
        => Sb.ToString();
}