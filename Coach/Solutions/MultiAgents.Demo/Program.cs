using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Agents.Chat;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using MultiAgents.Demo.Helpers;
using Spectre.Console;

Console.WriteLine("Hello Multi-Agent demo!");
var deploymentName = Environment.GetEnvironmentVariable("DEPLOYMENTNAME");
var apiKey = Environment.GetEnvironmentVariable("APIKEY");
var endpoint = Environment.GetEnvironmentVariable("ENDPOINT");

ArgumentException.ThrowIfNullOrEmpty(deploymentName);
ArgumentException.ThrowIfNullOrEmpty(apiKey);
ArgumentException.ThrowIfNullOrEmpty(endpoint);

AnsiConsole.WriteLine("Current configuration:");
AnsiConsole.MarkupLine($"[bold blue]ModelId:[/]{deploymentName}");
AnsiConsole.MarkupLine($"[bold blue]Environment:[/]{apiKey}");

var kernel = AgentHelpers.BuildKernel(deploymentName, endpoint, apiKey);
var weatherAgent = AgentHelpers.BuildAgent(kernel, "WeatherAgent",
    "You are an agent designed to query and retrieve information about the weather of a given location. Keep it short and concise such as it's 20 celsious sunny, hot, cold, rainy or cloud with no description. For now, just make it up, you do not have to call any service. Do not tell anything other than the weather.");

var foodAgentKernel = kernel.Clone();
foodAgentKernel.ImportPluginFromPromptDirectory("Plugins/FoodPlugin");
var foodAgent = AgentHelpers.BuildAgent(foodAgentKernel, "FoodAgent",
    "You are an agent designed to query and retrieve information about the food of a given location. Keep it short and concise such as it's a pizza, burger, salad, steak, or sushi. For now, just make it up, you do not have to call any service. Do not tell anything other than the food.");

var travelAgent = AgentHelpers.BuildAgent(kernel, "TravelAgent",
    "You are an agent responsible for suggesting activities and creating an itinerary for a given location with respect to the weather. Do not tell anything other than the travel plans. Your travel plans should not be more than 2 activities.");
var conciergeAgent = AgentHelpers.BuildAgent(kernel, "ConciergeAgent",
    "You are an experienced hotel concierge who is responsible for the overall destination experience for the guest. You should:\n" +
    "1. Review if the activities match the weather conditions. You also need to make sure that you get weather, food and travel plan before approving or rejecting the plan.\n" +
    "2. Ensure restaurant recommendations align with meal times and weather. If meal does not contain any vegetarian options then reject the plan.\n" +
    "3. Always provide your feedback but very concise, to the point. No extra chatty words.\n" +
    "4. You are only allowed to approve with a word approve or else provide your feedback as why it is not approved.");

var terminationFunctionInstructions =
    """
    Determine if the travel plan has been approved. Provide the reason of your decision as well.
    
     Respond in JSON format.  The JSON schema can include only:
    {
        "isApproved": "bool (true if the user request has been approved)",
        "reason": "string (the reason for your determination)"
    }

    History:
    {{$history}}
    """;

var selectionFunctionInstructions =
    $$$"""
       Determine which participant takes the next turn in a conversation based on the the most recent participant. 
       State only the name of the participant to take the next turn. 
       No participant should take more than one turn in a row.

       Choose only from these participants in the following order:
       - {{{weatherAgent.Name}}}
       - {{{foodAgent.Name}}}
       - {{{travelAgent.Name}}}
       - {{{conciergeAgent.Name}}}

       Always follow these rules when selecting the next participant:        
       After {{{weatherAgent.Name}}} has responded, select the {{{foodAgent.Name}}} agent for the next response.
       After {{{foodAgent.Name}}} has responded, select the {{{travelAgent.Name}}} agent for the next response.
       After {{{travelAgent.Name}}} has responded, select the {{{conciergeAgent.Name}}} agent for the next response.

       Based upon the feedback of ConciergeAgent's feedback, select the appropriate agent for the response. For example, if the suggestion is for food then it should be the {{{foodAgent.Name}}}.

       Respond in JSON format.  The JSON schema can include only:
       {
           "name": "string (the name of the agent selected for the next turn)",
           "reason": "string (the reason for the participant was selected)"
       }

       History:
       {{$history}}
       """;

AzureOpenAIPromptExecutionSettings jsonSettings = new()
#pragma warning disable SKEXP0010
    { ResponseFormat = OpenAI.Chat.ChatResponseFormat.CreateJsonObjectFormat() };
#pragma warning restore SKEXP0010
var selectionFunction = KernelFunctionFactory.CreateFromPrompt(selectionFunctionInstructions, jsonSettings);
var terminationFunction = KernelFunctionFactory.CreateFromPrompt(terminationFunctionInstructions, jsonSettings);
// Limit history used for selection and termination to the most recent message.
#pragma warning disable SKEXP0001
ChatHistoryTruncationReducer strategyReducer = new(5);
#pragma warning restore SKEXP0001
// Create a chat for agent interaction.
#pragma warning disable SKEXP0110
AgentGroupChat chat =
#pragma warning restore SKEXP0110
    new(weatherAgent, foodAgent, travelAgent, conciergeAgent)
    {
        ExecutionSettings =
            new()
            {
                //  // Here a KernelFunctionSelectionStrategy selects agents based on a prompt function.
                SelectionStrategy =
#pragma warning disable SKEXP0110
                    new KernelFunctionSelectionStrategy(selectionFunction, kernel)
#pragma warning restore SKEXP0110
                    {
                        // Always start with the weather agent
                        InitialAgent = weatherAgent,
                        // Returns the entire result value as a string.
                        ResultParser = result =>
                        {
                            var jsonResult =
                                JsonResultTranslator.Translate<AgentSelectionResult>(result.GetValue<string>());

                            var agentName = string.IsNullOrWhiteSpace(jsonResult?.name) ? null : jsonResult.name;
                            agentName ??= foodAgent.Name;

                            return agentName ?? string.Empty;
                        },
                        HistoryVariableName = "history",
                        // Save tokens by not including the entire history in the prompt
                        HistoryReducer = strategyReducer
                    },

                // Here KernelFunctionTerminationStrategy will terminate
                // when the concierge agent has given their approval.
                TerminationStrategy =
#pragma warning disable SKEXP0110
                    new KernelFunctionTerminationStrategy(terminationFunction, kernel)
#pragma warning restore SKEXP0110
                    {
                        // Only the concierge agent may approve.
                        Agents = [conciergeAgent],
                        // Customer result parser to determine if the response is "yes"
                        ResultParser =
                            result =>
                            {
                                var jsonResult =
                                    JsonResultTranslator.Translate<OuterTerminationResult>(result.GetValue<string>());

                                return jsonResult?.isApproved ?? false;
                            },
                        // The prompt variable name for the history argument.
                        HistoryVariableName = "history",
                        // Limit total number of turns
                        MaximumIterations = 10,
                        // Save tokens by not including the entire history in the prompt
                        HistoryReducer = strategyReducer
                    },
            }
    };

// Invoke chat and display messages.
ChatMessageContent input = new(AuthorRole.User,
    "I'm planning to spend a day in Athens tomorrow. Can you help me plan my day? I need to know about the weather, food options, and activities I can do.");
chat.AddChatMessage(input);

await foreach (var response in chat.InvokeAsync())
{
    // Include ChatMessageContent.AuthorName in output, if present.
#pragma warning disable SKEXP0001
    var authorExpression = response.Role == AuthorRole.User ? string.Empty : $" - {response.AuthorName ?? "*"}";
#pragma warning restore SKEXP0001
    AnsiConsole.WriteLine(authorExpression);
}

AnsiConsole.WriteLine($"\n[IS COMPLETED: {chat.IsComplete}]");