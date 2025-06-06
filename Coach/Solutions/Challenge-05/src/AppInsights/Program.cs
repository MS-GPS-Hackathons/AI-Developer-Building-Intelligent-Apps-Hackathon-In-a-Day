﻿using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;
using System;
using Microsoft.SemanticKernel.Embeddings;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using OpenTelemetry.Resources;
using Azure.Monitor.OpenTelemetry.Exporter;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

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

// Add Telemetry with App Insights

#region Telemetry
var connectionString = config.AppInsightsConnectionString;

var resourceBuilder = ResourceBuilder
    .CreateDefault()
    .AddService("TelemetryApplicationInsightsQuickstart");

// Enable model diagnostics with sensitive data.
AppContext.SetSwitch("Microsoft.SemanticKernel.Experimental.GenAI.EnableOTelDiagnosticsSensitive", true);

using var traceProvider = Sdk.CreateTracerProviderBuilder()
    .SetResourceBuilder(resourceBuilder)
    .AddSource("Microsoft.SemanticKernel*")
    .AddAzureMonitorTraceExporter(options => options.ConnectionString = connectionString)
    .Build();

using var meterProvider = Sdk.CreateMeterProviderBuilder()
    .SetResourceBuilder(resourceBuilder)
    .AddMeter("Microsoft.SemanticKernel*")
    .AddAzureMonitorMetricExporter(options => options.ConnectionString = connectionString)
    .Build();

using var loggerFactory = LoggerFactory.Create(builder =>
{
    // Add OpenTelemetry as a logging provider
    builder.AddOpenTelemetry(options =>
    {
        options.SetResourceBuilder(resourceBuilder);
        options.AddAzureMonitorLogExporter(options => options.ConnectionString = connectionString);
        // Format log messages. This is default to false.
        options.IncludeFormattedMessage = true;
        options.IncludeScopes = true;
    });
    builder.SetMinimumLevel(LogLevel.Information);
});
#endregion

// Create kernel
var builder = Kernel.CreateBuilder();

builder.Services.AddSingleton(loggerFactory);

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
history.AddSystemMessage("You are an AI assistant managing the lights and product information.");

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