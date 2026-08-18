#:package Azure.AI.OpenAI@2.9.0-beta.1
#:package Azure.Identity@1.21.0
#:package Microsoft.Extensions.AI@10.9.0
#:package Microsoft.Extensions.AI.OpenAI@10.3.0
#:package Microsoft.Extensions.Configuration@10.0.0
#:package Microsoft.Extensions.Configuration.EnvironmentVariables@10.0.0
#:package Microsoft.Extensions.Configuration.UserSecrets@10.0.0
#:property UserSecretsId=f8d4253e-5e0c-4aac-b93b-4e8e1d5d7b25

using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using Microsoft.Extensions.AI;

await FunctionSample.RunAsync();

public class FunctionSample
{
    public static async Task RunAsync()
    {
        var config = new ConfigurationBuilder()
            .AddUserSecrets<FunctionSample>()
            .AddEnvironmentVariables()
            .Build();

        var deploymentName = config["AZURE_OPENAI_DEPLOYMENT"] ?? "mai-thinking-1";
        var endpoint = new Uri(config["AZURE_AI_ENDPOINT"]
            ?? throw new InvalidOperationException(
                "AZURE_AI_ENDPOINT is not set. Run: dotnet user-secrets set \"AZURE_AI_ENDPOINT\" \"<your-endpoint>\""));

        IChatClient chatClient = new AzureOpenAIClient(endpoint, new DefaultAzureCredential())
            .GetChatClient(deploymentName)
            .AsIChatClient()
            .AsBuilder()
            .UseFunctionInvocation()
            .Build();

        var chatOptions = new ChatOptions
        {
            Tools = [AIFunctionFactory.Create(GetTicketStatus)]
        };

        var response = await chatClient.GetResponseAsync(
            "Ticket TCK-4471 has been open a while — what's going on and what should we do?",
            chatOptions);

        Console.WriteLine(response.Text);
    }

    // A stand-in for a real ticketing system lookup
    [Description("Looks up the current status and priority of a support ticket by ID.")]
    public static string GetTicketStatus(string ticketId)
    {
        return ticketId switch
        {
            "TCK-4471" => "Status: Escalated. Priority: Critical. Opened 6 days ago, SLA breached.",
            _ => "Ticket not found."
        };
    }
}