using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;

namespace WorkflowSample;

public static class WorkflowFactory {
    public static Workflow CreateWorkflow(IChatClient chatClient, string name) {
        ArgumentNullException.ThrowIfNull(chatClient);

        // var firstAgent = AgentFactory.CreateAgent(chatClient, "FirstAgent", "You are first agent in workflow for generation enriched prompt", AgentsPrompts.FirstAgentSystem);
        // var secondAgent = AgentFactory.CreateAgent(chatClient, "SecondAgent", "You are second agent in workflow for generation enriched prompt", AgentsPrompts.FirstAgentSystem);
        // var thirdAgent = AgentFactory.CreateAgent(chatClient, "ThirdAgent", "You are third agent in workflow for generation enriched prompt", AgentsPrompts.FirstAgentSystem);
        
        var firstAgent = AgentFactory.CreateAgent(chatClient, "FirstAgent", string.Empty, AgentsPrompts.FirstAgentSystem);
        var secondAgent = AgentFactory.CreateAgent(chatClient, "SecondAgent", string.Empty, AgentsPrompts.FirstAgentSystem);
        var thirdAgent = AgentFactory.CreateAgent(chatClient, "ThirdAgent", string.Empty, AgentsPrompts.FirstAgentSystem);

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