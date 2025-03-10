using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace MultiAgents.Helpers;

public class BudgetAdvisor
{
    [KernelFunction, Description("Evaluate the total cost of the itinerary")]
    [return: Description("over or under budget")]
    public Task<string> EvaluateTotalCostAsync(
        [Description("Total user's budget")] int userBudget,
        [Description("Estimated budget")] int estimatedBudget
    )
    {
        // If an estimated budget is over the user's budget, return that it is over budget
        return Task.FromResult(estimatedBudget > userBudget ? "over budget" : "under budget");
    }
}