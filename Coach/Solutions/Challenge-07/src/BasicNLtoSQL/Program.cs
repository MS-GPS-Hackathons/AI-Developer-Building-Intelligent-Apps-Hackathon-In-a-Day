using AIDevHackathon.ConsoleApp.BasicNLtoSQL.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Xml.Schema;

namespace AIDevHackathon.ConsoleApp.BasicNLtoSQL
{
#pragma warning disable SKEXP0001
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                //Initialize confguration from appsettings.json
                var azureConfig = new AzureConfiguration();

                //Create the kernel builder and add the Azure OpenAI chat completion plugin
                var builder = Kernel.CreateBuilder().AddAzureOpenAIChatCompletion(azureConfig.AOAIDeploymentId, azureConfig.AOAIEndpoint, azureConfig.AOAIKey);

                // Add logging
                var logger = new LoggerFactory().CreateLogger("Program");
                builder.Services.AddSingleton<ILogger>(logger);

                // Build the kernel
                var kernel = builder.Build();

                //Load the database schema
                var sqlSchema = await File.ReadAllTextAsync("Data\\dbschema.txt");


                ChatCompletionAgent agentNLtoSQL =
                new()
                {
                    Kernel = kernel,
                    Name = "NLtoSQL",
                    Instructions = @"You are an sql query assistant, you transform natural language to sql statements. 
                                     Given the following SQL schema and a query in natural language, you have to format the query into a single valid SQL statement. 
                                     
                                     {{$sqlSchema}}",

                    Arguments = new KernelArguments()
                    {
                        { "sqlSchema", sqlSchema }
                    }
                };

                AgentThread thread = new ChatHistoryAgentThread();

                // Start the conversation
                while (true)
                {
                    try
                    {
                        // Get user input
                        System.Console.Write("User > ");
                        var userInput = Console.ReadLine();

                        // Generate the agent response(s)
                        await foreach (ChatMessageContent response in agentNLtoSQL.InvokeAsync(new ChatMessageContent(AuthorRole.User, userInput), thread))
                        {
                            System.Console.Write(response.Content);
                        }                                      
                        System.Console.WriteLine();

                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}