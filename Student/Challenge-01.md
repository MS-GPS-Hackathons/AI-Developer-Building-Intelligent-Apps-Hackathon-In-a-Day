# Challenge 1 - Implement Retrieval Augmented Generation (RAG) with Azure OpenAI

 [< Previous Challenge](./Challenge-00.md) - **[Home](../README.md)** - [Next Challenge >](./Challenge-02.md)
 
## Introduction

As a developer embarking on a new startup venture, you have recently been inspired to explore building advanced Azure AI services to drive innovation.

As part of this initiative, you will be focusing on implementing Retrieval Augmented Generation (RAG) with Azure OpenAI. 

As a startup company, you have the opportunity to leverage cutting-edge AI technologies and figure out how to build intelligent applications.


## Description

You have conducted a thorough research and developed a comprehensive plan to achieve your goals by leveraging Azure OpenAI and Azure AI Search.

To begin, you will create an Azure OpenAI service instance and deploy two models:

- text-embedding-ada-002
- gpt-4o

The **text-embedding-ada-002** will be used as the "vector creation" / embeddings models and the **gpt-4o** as the core LLM for all upcoming tasks. 
Once confirmed, you will add your own data to the model and conduct testing to ensure it performs accurately with your specific information. 

You should complete the following steps:

  - Create an Azure OpenAI instance.
  - Deploy an Azure OpenAI **text-embedding-ada-002** model.
  - Deploy an Azure OpenAI **gpt-4o** model (version 0613 or later).
  - Use the playground to chat with the Gpt-4o model and explore basic functionality.
  - Create an Azure Storage account and upload your sample product information.
  - Create an Azure AI Search index that will be used for hybrid queries.
  - Create an Index on Azure AI Search, where you can do a hybrid search on your sample products.
  - Chat in the playground with your data.

You can find the sample product information data for grounding your own data [here](./Resources/Challenge-01/Data/product-info)

## Success Criteria

- Demonstrate that you can chat, without your data with a Gpt-4o Azure OpenAI model.
- Demonstrate that you have uploaded the sample product information data markup(md) files to an Azure Storage Account.
- Demonstrate that you can do a keyword search on the sample product markup files.
- Demonstrate that you can chat with your own data in AI Foundry Chat Playground with Hybrid Search.
- Demonstrate that you get an answer (on product information data) by using the prompt "How much are the TrailWalker hiking shoes".
  
## Learning Resources
- [Create and deploy an Azure OpenAI Service resource](https://learn.microsoft.com/en-us/azure/ai-services/openai/how-to/create-resource?pivots=web-portal)
- [Create an Azure storage account](https://learn.microsoft.com/en-us/azure/storage/common/storage-account-create?tabs=azure-portal)
- [Quickstart: Upload, download, and list blobs with the Azure portal](https://learn.microsoft.com/en-us/azure/storage/blobs/storage-quickstart-blobs-portal)
- [Quickstart: Vectorize text and images in the Azure portal](https://learn.microsoft.com/en-us/azure/search/search-get-started-portal-import-vectors?tabs=sample-data-storage%2Cmodel-aoai%2Cconnect-data-storage)
- [Azure OpenAI On Your Data](https://learn.microsoft.com/en-us/azure/ai-services/openai/concepts/use-your-data)
- [RAG and generative AI - Azure AI Search](https://learn.microsoft.com/en-us/azure/search/retrieval-augmented-generation-overview)
- [Hybrid search - Azure AI Search | Microsoft Learn](https://learn.microsoft.com/en-us/azure/search/hybrid-search-overview)
- [Azure OpenAI Service models](https://learn.microsoft.com/en-us/azure/ai-services/openai/concepts/models?tabs=python-secure%2Cglobal-standard%2Cstandard-chat-completions)
