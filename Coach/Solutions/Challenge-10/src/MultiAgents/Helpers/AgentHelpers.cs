using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Agents.Chat;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Spectre.Console;

namespace MultiAgents.Helpers;

/// <summary>
/// Provides helper methods to build and manage agents for the MultiAgents application.
/// </summary>
public class AgentHelpers
{
    /// <summary>
    /// Builds a Kernel instance configured with Azure OpenAI Chat Completion.
    /// </summary>
    /// <param name="deploymentName">The deployment name for the Azure OpenAI service.</param>
    /// <param name="endpoint">The endpoint URL for the Azure OpenAI service.</param>
    /// <param name="apiKey">The API key for the Azure OpenAI service.</param>
    /// <returns>A configured Kernel instance.</returns>
    public static Kernel BuildKernel(string deploymentName, string endpoint, string apiKey)
    {
        var builder = Kernel.CreateBuilder();
        builder.AddAzureOpenAIChatCompletion(deploymentName, endpoint, apiKey);
        builder.Services.AddLogging(services => services.AddConsole().SetMinimumLevel(LogLevel.Trace));
        var kernel = builder.Build();
        return kernel;
    }

    /// <summary>
    /// Builds a ChatCompletionAgent for travel planning.
    /// </summary>
    /// <param name="kernel">The Kernel instance to be used by the agent.</param>
    /// <returns>A configured ChatCompletionAgent for travel planning.</returns>
    public static ChatCompletionAgent BuildTravelAgent(Kernel kernel)
    {
        var travelPlannerAgent = new ChatCompletionAgent
        {
            Name = "TravelPlanner",
            Instructions =
                "You create detailed travel itineraries based on user preferences. You are very concise, to the point and do not waste time explaining things in detail. At the end of each plan, you also calculate your total and make sure you provide reasoning for that total budget. Consider suggestions when revising the travel plan.",
            Kernel = kernel
        };

        return travelPlannerAgent;
    }

    /// <summary>
    /// Builds a ChatCompletionAgent for budget advisory.
    /// </summary>
    /// <param name="kernel">The Kernel instance to be used by the agent.</param>
    /// <returns>A configured ChatCompletionAgent for budget advisory.</returns>
    public static ChatCompletionAgent BuildBudgetAdvisoryAgent(Kernel kernel)
    {
        var budgetAdvisorKernel = kernel.Clone();
        budgetAdvisorKernel.ImportPluginFromObject(new BudgetAdvisor());

        var budgetAdvisorAgent = new ChatCompletionAgent
        {
            Name = "BudgetAdvisor",
            Instructions =
                "You analyze travel plans and suggest adjustments to fit within the total cost. Your goal is to ensure that the user's travel plans are financially feasible and not beyond the total cost. If they are not within the range, provide feedback and reject the plan with 'no'. If it is within a range then approve it with 'yes'.",
            Kernel = budgetAdvisorKernel,
            Arguments = new KernelArguments(new AzureOpenAIPromptExecutionSettings
                { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() })
        };

        return budgetAdvisorAgent;
    }

    /// <summary>
    /// Invokes the agent asynchronously with the provided input and updates the chat history.
    /// </summary>
    /// <param name="input">The input provided by the user.</param>
    /// <param name="agent">The ChatCompletionAgent to be invoked.</param>
    /// <param name="chat">The chat history to be updated.</param>
    public async Task InvokeAgentWithInputFromUserAsync(string input, ChatCompletionAgent agent, ChatHistory chat)
    {
        ChatMessageContent message = new(AuthorRole.User, input);
        chat.Add(message);

        await foreach (ChatMessageContent response in agent.InvokeAsync(chat))
        {
            chat.Add(response);
            Console.WriteLine(response);
        }
    }

    /// <summary>
    /// execute flow where we have two agents in a chat and travel agents checks if the data is according to budget constraints
    /// </summary>
    /// <param name="travelPlannerAgent">travel agent</param>
    /// <param name="budgetAdvisorAgent">budget agent with constraints</param>
    public static async Task ExecuteFlowAsync(ChatCompletionAgent travelPlannerAgent,
        ChatCompletionAgent budgetAdvisorAgent)
    {
        // Create the AgentGroupChat with the agents and strategies
#pragma warning disable SKEXP0110
        var chat = new AgentGroupChat(travelPlannerAgent, budgetAdvisorAgent)
#pragma warning restore SKEXP0110
        {
            ExecutionSettings =
                new()
                {
                    //  // Here a KernelFunctionSelectionStrategy selects agents based on a prompt function.
#pragma warning disable SKEXP0110
                    SelectionStrategy = new SequentialSelectionStrategy
                    {
                        InitialAgent = travelPlannerAgent
                    },
#pragma warning restore SKEXP0110

                    // Here KernelFunctionTerminationStrategy will terminate
                    // when the concierge agent has given their approval.
                    TerminationStrategy =
#pragma warning disable SKEXP0110
                        new ApprovalTerminationStrategy()
#pragma warning restore SKEXP0110
                        {
                            // Only the budget advisory director may approve.
                            Agents = [budgetAdvisorAgent],
                            // Limit total number of turns
                            MaximumIterations = 10
                        }
                }
        };

        // User input to start the conversation
        var userMessage = new ChatMessageContent(AuthorRole.User,
            "I want to plan a trip to Athens for 6 days, staying in hotels. My budget is $1200.");
        chat.AddChatMessage(userMessage);

        // Invoke the chat and display the conversation
        await foreach (var response in chat.InvokeAsync())
        {
#pragma warning disable SKEXP0001
            var authorExpression = response.Role == AuthorRole.User ? string.Empty : $" - {response.AuthorName}";
#pragma warning restore SKEXP0001
            var contentExpression = string.IsNullOrWhiteSpace(response.Content) ? string.Empty : response.Content;
            AnsiConsole.WriteLine($"\n# {response.Role}{authorExpression}: {contentExpression}");
        }

        AnsiConsole.WriteLine($"\n[CONVERSATION COMPLETED: {chat.IsComplete}]");
    }
}

[Experimental("SKEXP0110")]
sealed class ApprovalTerminationStrategy : TerminationStrategy
{
    // Terminate when the final message contains the term "approve"
    protected override Task<bool> ShouldAgentTerminateAsync(Agent agent, IReadOnlyList<ChatMessageContent> history,
        CancellationToken cancellationToken)
        => Task.FromResult(history[^1].Content?.Contains("yes", StringComparison.OrdinalIgnoreCase) ?? false);
}
