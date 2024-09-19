using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Reviewabot.Console.GitHub.Models;

namespace Reviewabot.Console.GitHub;

public class PullRequest
{
    private readonly HttpClient _client;
    
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };
    
    public PullRequest(string token)
    {
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Add("User-Agent", "Reviewabot");
        _client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.diff");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<string> GetPrDiffAsync(string owner, string repo, string prNumber)
    {
        // https://docs.github.com/en/rest/pulls/pulls?apiVersion=2022-11-28#get-a-pull-request
        var response = await _client.GetAsync($"https://api.github.com/repos/{owner}/{repo}/pulls/{prNumber}");
        return await response.Content.ReadAsStringAsync();
    }

    public async Task ReviewPrAsync(string owner, string repo, string prNumber, string eventName, string review)
    {
        // https://docs.github.com/en/rest/pulls/reviews?apiVersion=2022-11-28#create-a-review-for-a-pull-request
        var reviewBody = new PullRequestReview { Body = review, Event = eventName };
        var response = await _client.PostAsJsonAsync($"https://api.github.com/repos/{owner}/{repo}/pulls/{prNumber}/reviews", reviewBody, _serializerOptions);
        response.EnsureSuccessStatusCode();
    }
}