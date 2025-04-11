# (Optional) Challenge 07 - Basic NL to SQL with semantic kernel.

 [< Previous Challenge](./Challenge-06.md) - **[Home](../README.md)** - [Next Challenge >](./Challenge-08.md)
 
## Introduction
The CTO is impressed with the results and the speed of implementation.

You have been assigned by the CTO to build your first agent and convert natural language queries into SQL statements. The objective is to precisely translate the user's intent into SQL queries that accurately retrieve the necessary data from the database.

An AI agent is a software entity designed to perform tasks autonomously or semi-autonomously by receiving input, processing information, and taking actions to achieve specific goals.

The Semantic Kernel Agent Framework provides a platform within the Semantic Kernel eco-system that allow for the creation of AI agents and the ability to incorporate agentic patterns into any application based on the same patterns and features that exist in the core Semantic Kernel framework.

## Description
In this challenge, you will practice converting natural language queries into SQL statements by using Semantic Kernel plugin. This exercise will help you understand how to translate user requests into precise SQL queries that can be executed on a database.

For this challenge you should add the Database Schema to Azure Open Ai context window.  You can find the database schema [here](./Resources/Challenge-07/dbschema.txt)

An agent can be configured directly using a Prompt Template Configuration, providing developers with a structured and reusable way to define its behavior. This approach offers a powerful tool for standardizing and customizing agent instructions, ensuring consistency across various use cases while still maintaining dynamic adaptability.

Creating an agent with template parameters provides greater flexibility by allowing its instructions to be easily customized based on different scenarios or requirements. This approach enables the agent's behavior to be tailored by substituting specific values or functions into the template, making it adaptable to a variety of tasks or contexts. By leveraging template parameters, developers can design more versatile agents that can be configured to meet diverse use cases without needing to modify the core logic, just use the curly braces {{...}} to embed expressions in your prompts.

To proceed with developing an ChatCompletionAgent, configure your development environment with the appropriate packages.
Add the Microsoft.SemanticKernel.Agents.Core package to your project:
``` bash
dotnet add package Microsoft.SemanticKernel.Agents.Core --prerelease
```

Initialize the Kernel with a chat-completion agent. Please note that the Instructions are using sqlSchema as a parameter
``` csharp
// Initialize a Kernel with a chat-completion service
IKernelBuilder builder = Kernel.CreateBuilder();

builder.AddAzureOpenAIChatCompletion(/*<...configuration parameters>*/);

Kernel kernel = builder.Build();

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
```

You should invoke the agent with the following code and use AgentThread to keep the conversation history
``` csharp
// Define agent
ChatCompletionAgent agentNLtoSQL = ...;

AgentThread thread = new ChatHistoryAgentThread();

// Generate the agent response(s)
await foreach (ChatMessageContent response in agentNLtoSQL.InvokeAsync(new ChatMessageContent(AuthorRole.User, "<user input>"), thread))
{
  // Process agent response(s)...
}
```

In the next challenge you will investigate alternative ways to optimize the solution. Discuss with your coach potential optimizations.

## Success Criteria
- Demonstrate that you have created the "Natural language to SQL" Semantic Kernel Prompt Plugin
- Demonstrate that you add the database schema in Azure Open AI Context window and you set meaningful instructions to the bot.
- Demonstrate that you can ask questions in natural language and you get responses with SQL queries.
- Discuss with your coach the disadvantages of current solution and propose ways to optimize this.

## Learning Resources
- [An Overview of the Agent Architecture](https://learn.microsoft.com/en-us/semantic-kernel/frameworks/agent/agent-architecture?pivots=programming-language-csharp)
- [Create an Agent from a Semantic Kernel Template](https://learn.microsoft.com/en-us/semantic-kernel/frameworks/agent/agent-templates?pivots=programming-language-csharp#agent-definition-from-a-prompt-template)
- [Exploring the Semantic Kernel ChatCompletionAgent](https://learn.microsoft.com/en-us/semantic-kernel/frameworks/agent/chat-completion-agent)
- [PromptTemplateConfig Class (Microsoft.SemanticKernel) | Microsoft Learn](https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.prompttemplateconfig?view=semantic-kernel-dotnet)
