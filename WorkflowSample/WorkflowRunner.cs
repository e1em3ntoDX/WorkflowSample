using System.Diagnostics;
using Microsoft.Agents.AI.Workflows;

namespace WorkflowSample;

public class WorkflowRunner {
    public async static Task<string> RunWorkflowAsync<TInput>(Workflow workflow, TInput input, CancellationToken cancellationToken) {
        ArgumentNullException.ThrowIfNull(workflow);

        string? result = null;
        
        var stopwatch = Stopwatch.StartNew();
        var lastElapsed = TimeSpan.Zero;


        await using StreamingRun run = await InProcessExecution.RunStreamingAsync(workflow, input, cancellationToken: cancellationToken);
        await run.TrySendMessageAsync(new TurnToken(emitEvents: true));

        Logger.LogHeader(workflow.Name ?? "Workflow Run", DateTime.UtcNow);
        
        await foreach (var @event in run.WatchStreamAsync(cancellationToken)) {
            var nowUtc = DateTime.UtcNow;
            var elapsed = stopwatch.Elapsed;
            var delta = elapsed - lastElapsed;
            lastElapsed = elapsed;

            Logger.LogWorkflowEvent(@event, nowUtc, elapsed, delta);

            switch (@event) {
                case AgentResponseEvent agentResponseEvent: 
                    break;
                case AgentResponseUpdateEvent agentResponseUpdateEvent:
                    break;
                case ExecutorInvokedEvent executorInvokedEvent:
                    break;
                case ExecutorCompletedEvent executorCompletedEvent:
                    break;
                case ExecutorFailedEvent executorFailedEvent:
                    break;
                case SuperStepCompletedEvent superStepCompletedEvent:
                    break;
                case RequestInfoEvent requestInfoEvent:
                    break;
                case SuperStepStartedEvent superStepStartedEvent:
                    break;
                case WorkflowStartedEvent workflowStartedEvent:
                    break;
                case WorkflowErrorEvent workflowErrorEvent:
                    break;
                case WorkflowWarningEvent workflowWarningEvent:
                    break;
                case WorkflowOutputEvent workflowOutputEvent:
                    result = workflowOutputEvent.Data as string;
                    break;
            }
        }

        Logger.LogFooter(workflow.Name ?? "Workflow Run", DateTime.UtcNow, stopwatch.Elapsed);
        
        return result ?? "Work flow result is null";
    }
}