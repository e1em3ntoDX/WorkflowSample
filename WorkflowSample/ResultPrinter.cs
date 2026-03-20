namespace WorkflowSample;

public static class ResultPrinter {
    public static void Print(string result) {
        var formatedResult = result.Replace("\n", Environment.NewLine);

        Console.WriteLine($"{AnsiColors.Green}++++++++++++++++++++++++++++++++++++++{AnsiColors.Reset}");
        Console.WriteLine($"{AnsiColors.Green}GENERATED REPORT:{AnsiColors.Reset}");
        Console.WriteLine();
        Console.WriteLine($"{AnsiColors.BrightYellow}{formatedResult}{AnsiColors.Reset}");
    }
}