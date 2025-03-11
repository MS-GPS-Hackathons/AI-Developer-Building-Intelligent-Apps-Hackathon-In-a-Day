# Challenge 02 - Use prompt flow to query on own data with Search AI.

 [< Previous Challenge](./Challenge-01.md) - **[Home](../README.md)** - [Next Challenge >](./Challenge-03.md)
 
## Introduction

Once you've finished the initial setup in the chat playground, it's time to explore deploying a real-time endpoint. You discover that using Prompt flow can help you accomplish this.

## Description

You figure out that Prompt flow is a tool that simplifies the entire development cycle of AI applications using Large Language Models (LLMs). It streamlines prototyping, experimenting, iterating, and deploying your AI applications.

By utilizing prompt flow, you're able to:
- Create executable flows that link LLMs, prompts, and Python tools through a visualized graph.
- Debug, share, and iterate your flows with ease through team collaboration.
- Create prompt variants and evaluate their performance through large-scale testing.
- Deploy a real-time endpoint that unlocks the full power of LLMs for your application.

You will use generative AI and prompt flow UI to build, configure, and deploy a copilot.

The copilot should answer questions about your products and services. For example, the copilot can answer questions such as "How much do the TrailWalker hiking shoes cost?"

You should complete the following steps
- Clone a chat prompt flow to ground your data
- Customize prompt flow and ground your data created in previous challenge.
- Deploy the flow for consumption.
- (Optional) Evaluate the flow using a question and answer evaluation dataset.

> [!NOTE]
> Clone the chat prompt flow "Multi-Round Q&A on Your Data" from the samples available

You can find the sample product information data used in previous challenge [here](./Resources/Challenge-01/Data/product-info)

## Success Criteria
- Demonstrate that you can chat within prompt flow with product info. Get answers to questions such as "Give me the description of the TrailWalker hiking shoes"
- Demonstrate that you deploy the flow and you can use the REST endpoint or the SDK to use the deployed flow.
- Describe the prompt flow steps to your coach.
- (Optional) Evaluate the flow using a question and answer evaluation dataset (here is the [eval_dataset](https://github.com/Azure-Samples/rag-data-openai-python-promptflow/blob/main/src/evaluation/evaluation_dataset.jsonl))
- (Optional) Show the evaluation status and results.

## Learning Resources

### Prompt flow
- [Prompt flow in Azure AI Studio - Azure AI Studio | Microsoft Learn](https://learn.microsoft.com/en-us/azure/ai-studio/how-to/prompt-flow)
- [How to build with prompt flow - Azure AI Studio | Microsoft Learn](https://learn.microsoft.com/en-us/azure/ai-studio/how-to/flow-develop)
- [Deploy a flow as a managed online endpoint for real-time inference - Azure AI Studio | Microsoft Learn](https://learn.microsoft.com/en-us/azure/ai-studio/how-to/flow-deploy)

### Evaluate models in Azure AI Studio
- [Evaluation of generative AI applications - Azure AI Studio | Microsoft Learn](https://learn.microsoft.com/en-us/azure/ai-studio/concepts/evaluation-approach-gen-ai)
- [How to evaluate generative AI apps with Azure AI Studio - Azure AI Studio | Microsoft Learn](https://learn.microsoft.com/en-us/azure/ai-studio/how-to/evaluate-generative-ai-app)
- [How to view evaluation results in Azure AI Studio - Azure AI Studio | Microsoft Learn](https://learn.microsoft.com/en-us/azure/ai-studio/how-to/evaluate-flow-results)
- [Monitoring evaluation metrics descriptions and use cases](https://learn.microsoft.com/en-us/azure/machine-learning/prompt-flow/concept-model-monitoring-generative-ai-evaluation-metrics?view=azureml-api-2)