using Microsoft.SemanticKernel; 
namespace Reviewabot.Console.Kernel;

public static class KernelFactory
{
    private const string ModelName = "gpt-4o";
    
    public static Microsoft.SemanticKernel.Kernel BuildKernel(string openAiKey, string azureOpenAiKey, string azureOpenAiEndpoint)
    {
        var handler = new HttpClientHandler();
        var client = new HttpClient(handler);

        if (!string.IsNullOrEmpty(openAiKey))
        {
            return Microsoft.SemanticKernel.Kernel.CreateBuilder()
                .AddOpenAIChatCompletion(ModelName, openAiKey, httpClient: client)
                .Build();
        }
        
        if (!string.IsNullOrEmpty(azureOpenAiKey) && !string.IsNullOrEmpty(azureOpenAiEndpoint))
        {
            return Microsoft.SemanticKernel.Kernel.CreateBuilder()
                .AddAzureOpenAIChatCompletion(ModelName, azureOpenAiEndpoint, azureOpenAiKey, httpClient: client)
                .Build();    
        }
        
        throw new InvalidOperationException("Either provide an OpenAI Key or an Azure OpenApi Key together with an Azure OpenAI Endpoint.");
    }
}
