using Microsoft.Agents.AI.Workflows;

namespace WorkflowSample;

public static class Logger {

    public static void LogWorkflowEvent( WorkflowEvent evt, DateTime nowUtc, TimeSpan elapsed, TimeSpan delta) {
        var eventName = evt.GetType().Name;
        var executorId = TryGetExecutorId(evt);
        var shortData = TryGetShortData(evt);
        
        Console.Write($"{AnsiColors.Gray}[{nowUtc:HH:mm:ss.fff} UTC]{AnsiColors.Reset}");
        Console.Write($"{AnsiColors.Blue}{eventName}{AnsiColors.Reset}");
        Console.Write($"{AnsiColors.Green} | +{elapsed.TotalMilliseconds,6:0} ms{AnsiColors.Reset}");
        Console.Write($"{AnsiColors.Yellow} | delta: {delta.TotalMilliseconds,6:0} ms{AnsiColors.Reset}");
        if (!string.IsNullOrWhiteSpace(executorId)) 
            Console.Write($"{AnsiColors.Blue} | executor: {executorId}{AnsiColors.Reset}");
        if (!string.IsNullOrWhiteSpace(shortData)) 
            Console.Write($"{AnsiColors.Gray} | data: {shortData}{AnsiColors.Reset}");
        if (evt is WorkflowErrorEvent workflowErrorEvent) 
            Console.Write($"{AnsiColors.Red} | error: {workflowErrorEvent.Exception?.Message}{AnsiColors.Reset}");
        if (evt is WorkflowWarningEvent workflowWarningEvent)
            Console.Write($"{AnsiColors.Yellow} | warning: {workflowWarningEvent.Data}{AnsiColors.Reset}");
        
        Console.WriteLine();
    }

    public static string? TryGetExecutorId(WorkflowEvent evt) {
        return evt switch {
            AgentResponseUpdateEvent e => e.ExecutorId,
            AgentResponseEvent e => e.ExecutorId,
            ExecutorInvokedEvent e => e.ExecutorId,
            ExecutorCompletedEvent e => e.ExecutorId,
            ExecutorFailedEvent e => e.ExecutorId,
            WorkflowOutputEvent e => e.ExecutorId,
            _ => null
        };
    }

    public static string? TryGetShortData(WorkflowEvent evt) {
        var data = evt switch {
            WorkflowOutputEvent e => e.Data,
            ExecutorCompletedEvent e => e.Data,
            _ => null
        };

        if (data is null)
            return null;

        var text = data.ToString();
        if (string.IsNullOrWhiteSpace(text))
            return data.GetType().Name;

        text = text.Replace("\r\n", " ").Replace("\n", " ").Trim();

        const int MAX = 120;
        return text.Length <= MAX ? text : string.Concat(text.AsSpan(0, MAX), "...");
    }

    public static void LogHeader(string title, DateTime startedUtc) {
        Console.WriteLine($"{AnsiColors.Magenta}=== RUN GENERATION ==={AnsiColors.Reset}");
        Console.WriteLine($"{AnsiColors.Magenta}=== {title} ==={AnsiColors.Reset}");
        Console.WriteLine($"{AnsiColors.Gray}[Started : {startedUtc:HH:mm:ss.fff} UTC]{AnsiColors.Reset}");
    }

    public static void LogFooter( string title, DateTime startedAtUtc, TimeSpan elapsed) {
        Console.WriteLine($"{AnsiColors.Magenta}=== FINISH GENERATION ==={AnsiColors.Reset}");
        Console.WriteLine($"{AnsiColors.Magenta}=== {title} ==={AnsiColors.Reset}");
        Console.WriteLine($"{AnsiColors.Gray}Started : {startedAtUtc:yyyy-MM-dd HH:mm:ss.fff} UTC{AnsiColors.Reset}");
        Console.WriteLine($"{AnsiColors.Gray}Elapsed : {elapsed.TotalMilliseconds:0} ms{AnsiColors.Reset}");
    }
}