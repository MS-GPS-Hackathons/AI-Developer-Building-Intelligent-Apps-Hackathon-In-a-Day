# Azure AI Developer Hackathon in a day for StartUps - Building Intelligent Apps

This hackathon will provide a deep dive experience targeted for developers by integrating AI solutions into applications. Hackathon is a collaborative learning experience, designed as a set of challenges to practice your technical skills. By participating in this hackathon, you will be able to understand the capabilities of integrating AI services into applications.

This workshop is structured as a half to full day event, totaling approximately 6 hours to complete all challenges. The actual time required depends on the attendees' skill levels.

The hackathon is a collaborative activity where attendees form teams of 3-5 people to go through each workshop.
  
## Learning Objectives
Upon completing the workshop, participants will be able to:
- Understand the fundamentals of Retrieval Augmented Generation (RAG) and its implementation using Azure OpenAI.
- Integrate Azure AI Search with RAG to enhance AI applications with contextually relevant data.
- Develop intelligent applications using Azure Open AI SDK.
- Develop intelligent applications using Semantic Kernel in either C# or Python, incorporating AI prompts seamlessly.
- Develop a Natural Language to SQL agent.
- Apply the learned concepts to create innovative solutions that address real-world challenges across industries.
  
## Prerequisites
- Familiarity with Azure services and the Azure portal.
- Basic understanding of AI and generative models.
- Experience in programming with C# or Python
- Your laptop (development machine): Win, MacOS or Linux that you have **administrator rights**.
- Active Azure Subscription with **Contributor access** to create or modify resources.
- Access to Azure OpenAI in the desired Azure subscription.
- Latest version of Azure CLI
- Latest version of Visual Studio or Visual Studio Code
- .NET 8.0 SDK or later version

## Target Audience
The intended audience are individuals with coding skills.
- AI Engineers
- Software Developers
- Solution Architects

## Challenges

---

### Challenge 0: **[Setup and prepare Environment](Student/Challenge-00.md)**

- Install the required development tools. This initial session is crucial to ensure that all participants are well-prepared and can fully engage with the workshop's content.

### Challenge 1: **[Implement Retrieval Augmented Generation (RAG) with Azure OpenAI](Student/Challenge-01.md)**

- Dive into the world of RAG and learn how to enhance your AI applications by integrating Azure OpenAI’s capabilities. This session will guide you through the process of implementing RAG with Azure AI Search, enabling your applications to leverage external data sources for grounded and contextually relevant responses.

### Challenge 2: **[Start coding with Azure OpenAI SDK and Inference SDK.](Student/Challenge-02.md)**

- Use Azure OpenAI or Inference SDK to start coding your intelligent apps. The Azure AI Foundry SDK is a comprehensive toolchain designed to simplify the development of AI applications on Azure. The SDKs are a set of client libraries that allows developers to interact with Azure AI Services.

### Challenge 3: **[Use Semantic Kernel as an Orchestrator to create a basic intelligent app.](Student/Challenge-03.md)**

- Unlock the potential of Semantic Kernel in developing intelligent applications. Semantic Kernel is a lightweight, open-source development kit that lets you easily build AI agents and integrate the latest AI models into your C#, Python, or Java codebase. It serves as an efficient middleware that enables rapid delivery of enterprise-grade solutions. It achieves this by allowing you to define plugins that can be chained together in just a few lines of code.

### (Optional) Challenge 4: **[Advanced Natural Language to SQL with Semantic Kernel and RAG](Student/Challenge-04.md)**

- Practice how to convert natural language queries into SQL statements by using Semantic Kernel. This challenge will help you understand how to translate user requests into precise SQL queries that can be executed on the database.

### (Optional) Challenge 5: **[Observability in Semantic Kernel](Student/Challenge-05.md)**

- When you build AI solutions, you want to be able to observe the behavior of your services. Observability is the ability to monitor and analyze the internal state of components within a distributed system. It is a key requirement for building enterprise-ready AI solutions.

### (Optional) Challenge 6: **[Deploy your intelligent app as a web chatbot](Student/Challenge-06.md)**

- Are you feeling too comfortable and eager to do more? This additional challenge will push your skills further by deploying your NL to SQL application to Azure as a web chatbot.

## References
- [Quickstart: Use the chat playground in Azure AI Foundry portal](https://learn.microsoft.com/en-us/azure/ai-foundry/quickstarts/get-started-playground)
- [Tutorial: Deploy an enterprise chat web app](https://learn.microsoft.com/en-us/azure/ai-foundry/tutorials/deploy-chat-web-app)
- [Implement Retrieval Augmented Generation (RAG) with Azure OpenAI](https://microsoftlearning.github.io/mslearn-openai/Instructions/Exercises/06-use-own-data.html)
- [Quickstart: Chat with Azure OpenAI models using your own data](https://learn.microsoft.com/en-us/azure/ai-services/openai/use-your-data-quickstart)
- [Getting Started with Semantic Kernel](https://learn.microsoft.com/en-us/semantic-kernel/get-started/quick-start-guide)
- [Develop AI agents using Azure OpenAI and the Semantic Kernel SDK - Training | Microsoft Learn](https://learn.microsoft.com/en-us/training/paths/develop-ai-agents-azure-open-ai-semantic-kernel-sdk/)
- [MSLearn - Develop AI Agents with Azure OpenAI and Semantic Kernel-SDK](https://github.com/MicrosoftLearning/MSLearn-Develop-AI-Agents-with-Azure-OpenAI-and-Semantic-Kernel-SDK/tree/main)
- [Develop AI apps using Azure AI services](https://learn.microsoft.com/en-us/azure/developer/ai/)

## Repository Contents

- `./Student`
  - Student's Challenge Guide
- `./Student/Resources`
  - Resource files, sample code, scripts, etc meant to be provided to students. (Must be packaged up by the coach and provided to students at start of event)
- `./Coach`
  - Coach's Guide and related files
- `./Coach/Solutions`
  - Solution files with completed example answers to a challenge

## Remarks
- Please note that the content of this workshop may become outdated, as Azure AI is a rapidly evolving platform. We recommend staying engaged with the Azure AI community for the most current updates and practices.
    
## Contributors
- Phanis Parpas
- Sakis Rokanas
- Rodanthi Alexiou
- Bojan Vrhovnik
