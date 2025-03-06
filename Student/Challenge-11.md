# Challenge 11 - Multiple Agents and execution strategies

[< Previous Challenge](./Challenge-10.md) - **[Home](../README.md)**

## Introduction

You have built different agents with different functionality to perform tasks required from your users.
Your managers gives you new assignment to integrate them to work together. In this challenge, you will build a
multi-agent system that uses
multiple agents to solve a problem with minimal human interaction.

Since you are going to offsite, managers asks you to leverage intelligence to go to location which are in specific
your coworkers budgets (travel and expenses). You need to build an agent which will now to call other agent to check if
the travel is in the boundaries of specific budget.

## Description

While a single agent can tackle specific tasks effectively, complex problems often demand collaboration. Multi-agent
systems introduce distributed intelligence, allowing agents to specialise and coordinate tasks in parallel. For example,
in a dynamic sales process, one agent could monitor social media trends, another analyses customer behavior and a third
provides actionable insights. This way, they work together for a holistic strategy.

Integrating multiple models into such systems amplifies their versatility. A single agent or even a group of agents tied
to one model can be limiting, particularly when diverse tasks demand specialised capabilities. By using multimodel
orchestration, agents to seamlessly use different models to get the most out of frontier models. For example, GPT-4
handles customer interactions, Llama 3.2 ensures real-time translations for global communication and .

Your task is to build a multi-agent system that uses multiple agents to solve a problem with minimal human interaction.

TODO: architecture and flow diagram

TODO: minimal code example to start with

## Success Criteria

- Demonstrate that you have built a multi-agent system that uses multiple agents to solve a problem with minimal human
  interaction.
- Explain to your coach the flow and strategy used.

## Learning Resources

- [Semantic Kernel C#](https://github.com/microsoft/semantic-kernel/blob/main/dotnet/README.md)
- [Agent Chat](https://learn.microsoft.com/en-us/semantic-kernel/frameworks/agent/examples/example-agent-collaboration?pivots=programming-language-csharp)
