using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables()
    .Build();

var deploymentName = config["AZURE_OPENAI_DEPLOYMENT"] ?? "mai-thinking-1";
var endpoint = new Uri(config["AZURE_AI_ENDPOINT"]
    ?? throw new InvalidOperationException(
        "AZURE_AI_ENDPOINT is not set. Run: dotnet user-secrets set \"AZURE_AI_ENDPOINT\" \"<your-endpoint>\""));

IChatClient chatClient = new AzureOpenAIClient(endpoint, new DefaultAzureCredential())
    .GetChatClient(deploymentName)
    .AsIChatClient();


var systemPrompt = """
    You are a contract risk analyst. When given a contract excerpt, you must:
    1. Identify every clause that creates financial or legal risk.
    2. Reason step-by-step about *why* each clause is risky before concluding.
    3. Rank the risks by severity (High / Medium / Low).
    4. Output a final structured summary — do not skip the reasoning steps.
    """;

var contractExcerpt = """
    Section 9.2: Client may terminate this Agreement for convenience with 5 days'
    written notice. Vendor shall be entitled to fees for Services rendered through
    the termination date only, with no early-termination compensation.

    Section 11.4: Vendor's total liability under this Agreement shall not exceed
    the total fees paid in the preceding twelve (12) months, except in cases of
    gross negligence, in which liability is uncapped.
    """;

var messages = new List<ChatMessage>
{
    new(ChatRole.System, systemPrompt),
    new(ChatRole.User, $"Analyze this contract excerpt:\n\n{contractExcerpt}")
};

var response = await chatClient.GetResponseAsync(messages);

Console.WriteLine(response.Text);