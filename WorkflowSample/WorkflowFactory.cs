using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;

namespace WorkflowSample;

public static class WorkflowFactory {
    public static Workflow CreateWorkflow(IChatClient chatClient, string name) {
        ArgumentNullException.ThrowIfNull(chatClient);

        var firstAgent = AgentFactory.CreateAgent(chatClient, "FirstAgent", "You are first agent in workflow for generation enriched prompt", "You are first agent in workflow for generation enriched prompt. You aim is analyze user prompt and extract valuable data");
        var secondAgent = AgentFactory.CreateAgent(chatClient, "SecondAgent", "You are second agent in workflow for generation enriched prompt", "You are second agent in workflow for generation enriched prompt. You aim is judge the data from first agent and decide is it suitable for report generation");
        var thirdAgent = AgentFactory.CreateAgent(chatClient, "ThirdAgent", "You are third agent in workflow for generation enriched prompt", "You are third agent in workflow for generation enriched prompt. You aim is create enriched prompt to generate report based on user prompt and data from first agent and the decision from second agent");

        var output = AgentFactory.CreateOutputExecutor();

        return new WorkflowBuilder(firstAgent)
            .AddEdge(firstAgent, secondAgent)
            .AddEdge(secondAgent, thirdAgent)
            .AddEdge(thirdAgent, output)
            .WithName(name)
            .WithOutputFrom(output)
            .Build();
    }
}