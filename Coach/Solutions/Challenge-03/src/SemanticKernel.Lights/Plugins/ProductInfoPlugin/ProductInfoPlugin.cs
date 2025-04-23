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

public class ProductInfoPlugin
{
#pragma warning disable SKEXP0001
    private readonly ITextEmbeddingGenerationService _textEmbeddingGenerationService;
    private readonly SearchIndexClient _indexClient;
    private readonly string _indexName;
    public ProductInfoPlugin(ITextEmbeddingGenerationService textEmbeddingGenerationService, SearchIndexClient indexClient, string indexName)
    {
        _textEmbeddingGenerationService = textEmbeddingGenerationService;
        _indexClient = indexClient;
        _indexName = indexName;
    }

    [KernelFunction("Search")]
    [Description("Search for product information to the given query.")]
    public async Task<string> SearchAsync(string query)
    {
        try
        {
            // Convert string query to vector
            var embedding = await _textEmbeddingGenerationService.GenerateEmbeddingAsync(query);

            // Get client for search operations
            var searchClient = _indexClient.GetSearchClient(_indexName);

            // Configure request parameters
            VectorizedQuery vectorQuery = new(embedding);
            vectorQuery.Fields.Add("text_vector");

            SearchOptions searchOptions = new() { VectorSearch = new() { Queries = { vectorQuery } } };

            // Perform search request
            Response<SearchResults<IndexSchema>> response = await searchClient.SearchAsync<IndexSchema>(searchOptions);

            // Collect search results
            await foreach (SearchResult<IndexSchema> result in response.Value.GetResultsAsync())
            {
                return result.Document.Content; // Return text from first result
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during search: {ex.Message}");
        }        

        return string.Empty;
    }

    private sealed class IndexSchema
    {
        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("contentVector")]
        public ReadOnlyMemory<float> ContentVector { get; set; }
    }
}