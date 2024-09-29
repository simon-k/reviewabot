using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Reviewabot.Console.GitHub;
using Reviewabot.Console.Kernel;

Console.WriteLine("Reviewabot");

// Build a configuration object from command line
IConfiguration config = new ConfigurationBuilder()
    .AddCommandLine(args)
    .Build();

var owner = config["Owner"] ?? throw new InvalidOperationException("Owner is not configured.");
var repo = config["Repo"] ?? throw new InvalidOperationException("Repo is not configured.");
var prNumber = config["PrNumber"] ?? throw new InvalidOperationException("PrNumber is not configured.");
var gitHubPat = config["GitHubPat"] ?? throw new InvalidOperationException("GitHubPat is not configured.");
var openAiKey = config["OpenAiKey"] ?? string.Empty;
var azureOpenAiKey = config["AzureOpenAiKey"] ?? string.Empty;
var azureOpenAiEndpoint = config["AzureOpenAiEndpoint"] ?? string.Empty;

// Print the configuration for debugging purposes. 
Console.WriteLine($"Owner: {owner}");
Console.WriteLine($"Repo: {repo}");
Console.WriteLine($"PrNumber: {prNumber}");
Console.WriteLine("GitHubPat: ***");
Console.WriteLine("OpenAiKey: {0}", string.IsNullOrEmpty(openAiKey) ? "Not specified" : "***");
Console.WriteLine("AzureOpenAiKey: {0}", string.IsNullOrEmpty(azureOpenAiKey) ? "Not specified" : "***");
Console.WriteLine("AzureOpenAiEndpoint: {0}", string.IsNullOrEmpty(azureOpenAiEndpoint) ? "Not specified" : azureOpenAiEndpoint);

var kernel = KernelFactory.BuildKernel(openAiKey, azureOpenAiKey, azureOpenAiEndpoint);
var settings = new OpenAIPromptExecutionSettings()
{
    Temperature = 0f
    //TODO: Try to experiment with the response format by setting it to json
}; 

var chatService = kernel.GetRequiredService<IChatCompletionService>();
var chatHistory = new ChatHistory("You are a friendly agent only does code reviews of pull requests on Github. " +
                                  "Only comment on changes that are not ok. " +
                                  "There should be no TODO comments in the code. " +
                                  "Consider if the code is clean and easy to read. " +
                                  "Consider too many linebreaks. " +
                                  "Consider naming of classes, functions, and variables. " +
                                  "Consider known security issues like secrets in clear text. " +
                                  "Files should always end with a newline. ");

Console.WriteLine($"Get the PR. Owner: {owner}, Repo: {repo}, PR Number: {prNumber}");
var pr = new PullRequest(gitHubPat);
var diff = await pr.GetPrDiffAsync(owner, repo, prNumber);

Console.WriteLine($"PR Diff: {Environment.NewLine}{diff}");

chatHistory.AddUserMessage($"Review this PR: {Environment.NewLine}{diff}");
var review = await chatService.GetChatMessageContentAsync(chatHistory, settings);

Console.WriteLine($"Review: {Environment.NewLine}{review}" );

Console.WriteLine("Submit the review");
await pr.ReviewPrAsync(owner, repo, prNumber, "APPROVE", review.ToString());
