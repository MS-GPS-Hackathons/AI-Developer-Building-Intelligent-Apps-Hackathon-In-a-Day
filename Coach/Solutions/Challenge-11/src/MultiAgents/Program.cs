//Microsoft.SemanticKernel.Agents.Abstractions, 1.14.1-alpha
//Microsoft.SemanticKernel.Agents.Core, 1.14.1-alpha
//Microsoft.SemanticKernel.Agents.OpenAI, 1.14.1-alpha
//Microsoft.SemanticKernel.Connectors.OpenAI, 1.14.1-alpha

using MultiAgents.Helpers;
using Spectre.Console;

AnsiConsole.WriteLine("Hello Multiple Agents!");

var deploymentName = Environment.GetEnvironmentVariable("DEPLOYMENTNAME");
var apiKey = Environment.GetEnvironmentVariable("APIKEY");
var endpoint = Environment.GetEnvironmentVariable("ENDPOINT");

ArgumentException.ThrowIfNullOrEmpty(deploymentName);
ArgumentException.ThrowIfNullOrEmpty(apiKey);
ArgumentException.ThrowIfNullOrEmpty(endpoint);

AnsiConsole.WriteLine("Current configuration:");
AnsiConsole.MarkupLine($"[bold blue]ModelId:[/]{deploymentName}");
AnsiConsole.MarkupLine($"[bold blue]Environment:[/]{apiKey}");

var kernel = AgentHelpers.BuildKernel(deploymentName, apiKey, endpoint);
var travelPlannerAgent = AgentHelpers.BuildTravelAgent(kernel);
var budgetAdvisorAgent = AgentHelpers.BuildBudgetAdvisoryAgent(kernel);
await AgentHelpers.ExecuteFlowAsync(travelPlannerAgent, budgetAdvisorAgent);