namespace WorkflowSample;

public static class ResultPrinter {
    public static void Print(string result) {
        var formatedResult = result.Replace("\n", Environment.NewLine);

        Console.WriteLine("\u001b[32m=== GENERATED REPORT ===\u001b[0m");
        Console.WriteLine("\u001b[33m" + formatedResult + "\u001b[0m");
        Console.WriteLine("\u001b[32m=== FINISH ===\u001b[0m");
    }
}