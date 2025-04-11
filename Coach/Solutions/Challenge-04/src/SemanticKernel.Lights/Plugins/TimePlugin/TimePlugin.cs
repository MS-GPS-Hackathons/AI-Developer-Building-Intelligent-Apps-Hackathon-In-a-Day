using System;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Embeddings;

public class TimePlugin
{

    [KernelFunction("Time")]
    [Description("Get the current date and time")]
    public async Task<DateTime> GetCurrentDateTime()
    {
        return DateTime.Now;
    }
}