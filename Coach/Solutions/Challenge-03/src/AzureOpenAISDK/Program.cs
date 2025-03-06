// Install the .NET library via NuGet: dotnet add package Azure.AI.OpenAI --prerelease  
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using AIDevHackathon.ConsoleApp.AzureOpenAISDK;
using Azure;
using Azure.AI.OpenAI;
using Azure.AI.OpenAI.Chat;
using OpenAI.Chat;
using static System.Environment;

// Disable the prerelease warning
#pragma warning disable AOAI001

//Read the configuration from the appsettings.json file
var config = new AzureConfiguration();

//Get the configuration values
var endpoint = config.AOAIEndpoint;
var deploymentName = config.AOAIDeploymentId;
var openAiApiKey = config.AOAIKey;

var searchEndpoint = config.SearchEndpoint;
var searchIndex = config.SearchIndex; 
var searchApiKey = config.SearchKey;

//Create an instance of the AzureOpenAIClient
AzureOpenAIClient azureClient = new(
    new Uri(endpoint),
    new ApiKeyCredential(openAiApiKey));
var chatClient = azureClient.GetChatClient(deploymentName);

//Add chat completion options with data source 
var options = new ChatCompletionOptions();
options.AddDataSource(new AzureSearchChatDataSource
{
    Endpoint = new Uri(searchEndpoint),
    IndexName = searchIndex,
    Authentication = DataSourceAuthentication.FromApiKey(searchApiKey),
});

//Add system message and user question
List<ChatMessage> messages =
[
    ChatMessage.CreateSystemMessage("You are an AI assistant that helps people find product information.")
];

Console.WriteLine("Type your question here: ");

while (true)
{
    Console.WriteLine();
    Console.Write("User: ");
    messages.Add(ChatMessage.CreateUserMessage(Console.ReadLine()));

    //Call the chat client to get the response
    ChatCompletion completion = chatClient.CompleteChat(messages, options);

    //Display the response and add it to chat history
    var response = completion.Content[0].Text;
    ChatMessage.CreateAssistantMessage(response);
    Console.WriteLine($"System: {response}");
}