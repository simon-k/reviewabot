using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Reviewabot.Console.GitHub;

Console.WriteLine("Reviewabot");

// Build a configuration object from command line
IConfiguration config = new ConfigurationBuilder()
    .AddCommandLine(args)
    .Build();

var owner = config["Owner"] ?? throw new InvalidOperationException("Owner is not configured.");
var repo = config["Repo"] ?? throw new InvalidOperationException("Repo is not configured.");
var prNumber = config["PrNumber"] ?? throw new InvalidOperationException("PrNumber is not configured.");
var openApiApiKey = config["OpenApiKey"] ?? throw new InvalidOperationException("OpenApiKey is not configured.");
var gitHubPat = config["GitHubPat"] ?? throw new InvalidOperationException("GitHubPat is not configured.");

// Print the configuration for debugging purposes. 
Console.WriteLine($"Owner: {owner}");
Console.WriteLine($"Repo: {repo}");
Console.WriteLine($"PrNumber: {prNumber}");
Console.WriteLine("OpenApiKey: ***");
Console.WriteLine("GitHubPat: ***");

// Bootstrapping the kernel
var handler = new HttpClientHandler();
var client = new HttpClient(handler);

var settings = new OpenAIPromptExecutionSettings()
{
    Temperature = 0f
    //TODO: Try to experiment with the response format by setting it to json
}; 

var kernel = Kernel.CreateBuilder()
    .AddOpenAIChatCompletion("gpt-4o", openApiApiKey, httpClient: client)
    .Build();
    
var chatService = kernel.GetRequiredService<IChatCompletionService>();
var chatHistory = new ChatHistory("You are a friendly agent only does code reviews of pull requests on Github. " +
                                  "Only comment on changes that are not ok. " +
                                  "There should be no TODO comments in the code. " +
                                  "Consider if the code is clean and easy to read. " +
                                  "Consider too many linebreaks. " +
                                  "Consider naming of functions and classes. " +
                                  "Files should always end with a newline. ");

Console.WriteLine($"Get the PR. Owner: {owner}, Repo: {repo}, PR Number: {prNumber}");
var pr = new PullRequest(gitHubPat);
var diff = await pr.GetPrDiffAsync(owner, repo, prNumber);

chatHistory.AddUserMessage($"Review this PR: {Environment.NewLine}{diff}");
var review = await chatService.GetChatMessageContentAsync(chatHistory, settings);

Console.WriteLine($"Review: {Environment.NewLine}{review}" );

Console.WriteLine("Submit the review");
await pr.ReviewPrAsync(owner, repo, prNumber, "APPROVE", review.ToString());
