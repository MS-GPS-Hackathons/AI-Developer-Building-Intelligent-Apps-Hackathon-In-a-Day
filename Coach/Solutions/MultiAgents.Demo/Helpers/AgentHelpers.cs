using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;

namespace MultiAgents.Demo.Helpers;

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
    /// Builds a ChatCompletionAgent with name and instructions.
    /// </summary>
    /// <param name="kernel">The Kernel instance to be used by the agent.</param>
    /// <param name="name">name of the agent</param>
    /// <param name="instructions">instructions for the agent</param>
    /// <returns>A configured ChatCompletionAgent for travel planning.</returns>
    public static ChatCompletionAgent BuildAgent(Kernel kernel, string name, string instructions)
    {
        var agent = new ChatCompletionAgent { Name = name, Instructions = instructions };
        return agent;
    }

    /// <summary>
    /// Invokes the agent asynchronously with the provided input and updates the chat history.
    /// </summary>
    /// <param name="input">The input provided by the user.</param>
    /// <param name="agent">The ChatCompletionAgent to be invoked.</param>
    /// <param name="chat">The chat history to be updated.</param>
    public static async Task InvokeAgentWithInputFromUserAsync(string input, ChatCompletionAgent agent, ChatHistory chat)
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
