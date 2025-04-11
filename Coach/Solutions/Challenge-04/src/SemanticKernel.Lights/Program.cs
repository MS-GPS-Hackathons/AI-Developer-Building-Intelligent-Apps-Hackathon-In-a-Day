using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;
using System;
using Microsoft.SemanticKernel.Embeddings;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;

#pragma warning disable SKEXP0010
var config = new AzureConfiguration();

var aoaiModelId = config.AOAIDeploymentId;
var aoaiEndpoint = config.AOAIEndpoint;
var aoaiApiKey = config.AOAIKey;
var aoaiEmbeddingsEndpoint = config.AOAIEmbeddingsEndpoint;
var aoaiEmbeddingsDeploymentId = config.AOAIEmbeddingsDeploymentId;
var searchEndpoint = config.SearchEndpoint;
var searchKey = config.SearchKey;
var searchIndexProducts = config.SearchIndexProducts;

// Create kernel
var builder = Kernel.CreateBuilder();
builder.AddAzureOpenAIChatCompletion(aoaiModelId, aoaiEndpoint, aoaiApiKey);

builder.AddAzureOpenAITextEmbeddingGeneration(
    deploymentName: aoaiEmbeddingsDeploymentId,
    endpoint: aoaiEmbeddingsEndpoint,
    apiKey: aoaiApiKey
);

//Disable the experimental warning
#pragma warning disable SKEXP0001

builder.Services.AddLogging(services => services.AddConsole().SetMinimumLevel(LogLevel.Trace));

var kernel = builder.Build();

// Retrieve the chat completion service
var chatCompletionService = kernel.Services.GetRequiredService<IChatCompletionService>();

// Add the lights plugin to the kernel
kernel.Plugins.AddFromType<LightsPlugin>("Lights");

// Add time plugin
kernel.Plugins.AddFromType<TimePlugin>("Time");

//Add Product Info Plugin
var searchClient = new SearchIndexClient(new Uri(searchEndpoint), new Azure.AzureKeyCredential(searchKey));
var textEmbeddingService = kernel.Services.GetRequiredService<ITextEmbeddingGenerationService>();

var productInfoPlugin = new ProductInfoPlugin(textEmbeddingService, searchClient, searchIndexProducts);

kernel.Plugins.AddFromObject(productInfoPlugin);

// Configure the execution settings with auto function calling
OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
{
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
};

// Create chat history
var history = new ChatHistory();
history.AddSystemMessage("You are an AI assistant managing the lights, time and product information.");

while (true)
{
    Console.Write("User: ");
    history.AddUserMessage(Console.ReadLine());

    // Get the response from the AI
    var result = await chatCompletionService.GetChatMessageContentAsync(
    history,
    executionSettings: openAIPromptExecutionSettings,
    kernel: kernel
    );

    Console.WriteLine($"Bot: {result}");
}