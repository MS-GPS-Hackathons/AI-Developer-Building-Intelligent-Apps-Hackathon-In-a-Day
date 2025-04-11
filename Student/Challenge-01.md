# Challenge 1 - Implement Retrieval Augmented Generation (RAG) with Azure OpenAI

 [< Previous Challenge](./Challenge-00.md) - **[Home](../README.md)** - [Next Challenge >](./Challenge-02.md)
 
## Introduction

As a developer embarking on a new startup venture, you have recently been inspired to explore building advanced Azure AI services to drive innovation.

As part of this initiative, you will be focusing on implementing Retrieval Augmented Generation (RAG) with Azure OpenAI. 

As a startup company, you have the opportunity to leverage cutting-edge AI technologies and figure out how to build intelligent applications.


## Description

You have conducted a thorough research and developed a comprehensive plan to achieve your  goals by leveraging Azure AI Foundry and deploying an Enterprise Chat web app. 

To begin, you will create a project in Azure AI Foundry and deploy an Azure OpenAI model. This will allow you to chat in the playground without using your own data, ensuring a smooth initial setup. 
Once confirmed, you will add your own data to the model and conduct testing to ensure it performs accurately with your specific information. 

Finally, you will deploy the web app, providing a robust and interactive chat solution tailored to your needs.
This structured approach ensures that each step is carefully designed, leading to a successful implementation.

You should complete the following steps:

  - Create an Azure AI Foundry project.
  - Deploy an Azure OpenAI model (version 0613 or later).
  - Chat in the playground without your data.
  - Add your data, create an Azure AI Search index that will use hybrid queries.
  - Chat in the playground with your data.
  - (Optional) Deploy your web app  

You can find the sample product information data for grounding your own data [here](./Resources/Challenge-01/Data/product-info)

## Success Criteria

- Demonstrate that you can chat with your own data in AI Foundry Chat Playground with Hybrid Search.
- Demonstrate that you get an answer (on product information data) by using the prompt "How much are the TrailWalker hiking shoes".
- (Optional) Deploy the web app and demonstrate that you can chat on your own data in the deployed app.
  
## Learning Resources
- [Quickstart: Use the chat playground in Azure AI Foundry portal](https://learn.microsoft.com/en-us/azure/ai-studio/quickstarts/get-started-playground)
- [Deploy an Enterprise Chat web app](https://learn.microsoft.com/en-us/azure/ai-studio/tutorials/deploy-chat-web-app)
- [RAG and generative AI - Azure AI Search](https://learn.microsoft.com/en-us/azure/search/retrieval-augmented-generation-overview)
- [Hybrid search - Azure AI Search | Microsoft Learn](https://learn.microsoft.com/en-us/azure/search/hybrid-search-overview)
- [Azure OpenAI Service models](https://learn.microsoft.com/en-us/azure/ai-services/openai/concepts/models?tabs=python-secure%2Cglobal-standard%2Cstandard-chat-completions)
