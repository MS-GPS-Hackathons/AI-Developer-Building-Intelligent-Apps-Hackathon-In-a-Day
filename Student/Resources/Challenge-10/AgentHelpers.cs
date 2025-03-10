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
}

