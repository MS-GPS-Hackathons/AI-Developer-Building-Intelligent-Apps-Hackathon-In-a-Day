# Challenge 2 - Start coding with Azure OpenAI SDK.

 [< Previous Challenge](./Challenge-01.md) - **[Home](../README.md)** - [Next Challenge >](./Challenge-03.md)

## Introduction
After completing the previous challenge, it's time to start investigating, how you can write code to utilize the AI services.

The Azure OpenAI SDK is a set of client libraries that allows developers to interact with Azure Open AI models. This service provides access to powerful Open AI foundational models, with the security and enterprise capabilities of Azure.

## Description
Now you’ll dive into .NET Core development by creating a simple console application and use your own data with Azure OpenAI models created in previous challenge.

You can also use the Azure Open AI SDK of another programming language of your preference.

The following example demonstrates the use of the Azure Open AI SDK.

In a console window use the dotnet new command to create a new console app.

```bash
dotnet new console -n azure-openai-sdk-hackathon
```

Install the OpenAI .NET client library with:

```bash
dotnet add package Azure.AI.OpenAI
```

Add using statements:

```bash
using OpenAI.Chat;
using Azure.AI.OpenAI;
```

Add your environmental variables
```csharp
string endpoint = "<Add AOAI GPT Enpoint>";
string deploymentName = "<Add AOAI GPT Key>";
string openAiApiKey = "<Add AOAI GPT Deployment name>";

string searchEndpoint = "<Add Azure Search Enpoint>";
string searchIndex = "<Add Azure Search Key>";
string searchApiKey = "<Add Azure Search Index for eCommerce products>";
```

Create client with an API key.

> [!NOTE]
> While this is not as secure as Microsoft Entra-based authentication, it's possible to authenticate using a client subscription key. Avoid authenticating with api keys on production environments.

```csharp
AzureOpenAIClient azureClient = new(
    new Uri(endpoint),
    new ApiKeyCredential(openAiApiKey));
ChatClient chatClient = azureClient.GetChatClient(deploymentName);
```

Use your own data with Azure OpenAI

```csharp
#pragma warning disable AOAI001

//Add chat completion options with data source 
ChatCompletionOptions options = new ChatCompletionOptions();
options.AddDataSource(new AzureSearchChatDataSource()
{
    Endpoint = new Uri(searchEndpoint),
    IndexName = searchIndex,
    Authentication = DataSourceAuthentication.FromApiKey(searchApiKey),
});

//Add system message and user question
List<ChatMessage> messages = new List<ChatMessage>();
messages.Add(ChatMessage.CreateSystemMessage("You are an AI assistant that helps people find product information."));
messages.Add(ChatMessage.CreateUserMessage("<Type your question here.>"));

ChatCompletion completion = chatClient.CompleteChat(messages, options);
```

Use the above code as an example and do all necessary changes to meet the success criteria.
You should make your application acting like a chat bot by adding conversation history as context for the subsequent calls.

## Success Criteria

- Demonstrate that the user can ask questions on your own data within the application.
- Demonstrate that you set the behavior of the bot as a product information application.
- Demonstrate that you use the conversation history as context for the subsequent calls.
- Discuss with your coach alternative ways authenticating with Azure AI services.
- (Optional) Demonstrate that you can stream the response chat messages.
  
## Learning Resources

- [Azure OpenAI client library for .NET - Azure for .NET Developers | Microsoft Learn](https://learn.microsoft.com/en-us/dotnet/api/overview/azure/ai.openai-readme?view=azure-dotnet)
- [Azure OpenAI client library for .NET - Use your own data | Microsoft Learn](https://learn.microsoft.com/en-us/dotnet/api/overview/azure/ai.openai-readme?view=azure-dotnet-preview#use-your-own-data-with-azure-openai)
- [Authenticate requests to Azure AI services](https://learn.microsoft.com/en-us/azure/ai-services/authentication)
- [Azure AI services authentication and authorization using .NET](https://learn.microsoft.com/en-us/dotnet/ai/azure-ai-services-authentication)
