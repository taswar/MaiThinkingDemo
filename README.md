# MAI-Thinking-1 C# Sample

![MAI-Thinking-1 for C# Developers](https://taswar.zeytinsoft.com/wp-content/uploads/2026/08/MAI-Thinking-1-for-C-Developers.png)

[![Build](https://github.com/taswar/MaiThinkingDemo/actions/workflows/dotnet.yml/badge.svg)](https://github.com/taswar/MaiThinkingDemo/actions/workflows/dotnet.yml)
![.NET 10](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet&logoColor=white)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen)](CONTRIBUTING.md)

- 📖 Full walkthrough: [MAI-Thinking-1 Is Now in Foundry — Here’s What It Means If You Write C#](https://taswar.zeytinsoft.com/mai-thinking-1-csharp-developers-guide/)
- ⭐ If this sample helps you, star the repo so other .NET developers can find it.
- 🔁 Related: [MAI-Image-2.5-Pro C# Sample](https://github.com/taswar/MAI-Image-2.5-Pro-Sample) and [MAI-Voice-2 Flash C# Samples](https://github.com/taswar/Mai-Voice-2-Flash-Sample)
- For more C# samples using other AI models, see [AI models for C# developers](https://taswar.zeytinsoft.com/foundry-ai-models-csharp/)
- For my free ebook on prompt engineering, see [Prompt Engineering for .NET Developers](https://github.com/taswar/PromptEngineeringForDotNetDevelopers)

A minimal .NET 10 console sample for **MAI-Thinking-1**, Microsoft's reasoning model on Microsoft Foundry. It calls the model's Azure OpenAI endpoint through the `Microsoft.Extensions.AI` `IChatClient` abstraction, authenticated with `DefaultAzureCredential`. It includes two independent samples:

- **[Program.cs](Program.cs)** — the main project. Sends a contract excerpt to the model for step-by-step risk analysis using `IChatClient`.
- **[FunctionSample.cs](FunctionSample.cs)** — a standalone, file-based app (no project needed) that also uses `IChatClient` and demonstrates automatic tool/function calling (`GetTicketStatus`) via `UseFunctionInvocation()`.

## Quick start

```powershell
git clone https://github.com/taswar/MaiThinkingDemo.git
cd MaiThinkingDemo
dotnet user-secrets set "AZURE_AI_ENDPOINT" "https://<your-resource>.openai.azure.com/"
az login
dotnet run
```

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- An Azure OpenAI or Microsoft Foundry resource with a MAI-Thinking-1 deployment
- Azure credentials available to `DefaultAzureCredential` (e.g. run `az login`, or use a Managed Identity / environment variables — see [DefaultAzureCredential docs](https://learn.microsoft.com/dotnet/api/azure.identity.defaultazurecredential))

## Configuration

Both samples read the same two settings:

| Setting | Description | Default |
| --- | --- | --- |
| `AZURE_AI_ENDPOINT` | Your Azure OpenAI endpoint URL (required) | — |
| `AZURE_OPENAI_DEPLOYMENT` | The chat model deployment name | `mai-thinking-1` |

Configuration is loaded from **user secrets** first, then **environment variables**.

Set the endpoint via user secrets (recommended for local dev):

```powershell
dotnet user-secrets set "AZURE_AI_ENDPOINT" "https://<your-resource>.openai.azure.com/"
dotnet user-secrets set "AZURE_OPENAI_DEPLOYMENT" "<your-deployment-name>"
```

Or via environment variables:

```powershell
$env:AZURE_AI_ENDPOINT = "https://<your-resource>.openai.azure.com/"
$env:AZURE_OPENAI_DEPLOYMENT = "<your-deployment-name>"
```

## Running the main project (Program.cs)

From the repository root:

```powershell
dotnet run
```

This builds and runs the `MaiThinkingDemo` project, which only compiles `Program.cs` (`FunctionSample.cs` is excluded from the project via `<Compile Remove="FunctionSample.cs" />` in [MaiThinkingDemo.csproj](MaiThinkingDemo.csproj)).

## Running the standalone sample (FunctionSample.cs)

`FunctionSample.cs` is a [.NET 10 file-based app](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-10/overview#file-based-apps) — it declares its own NuGet package references via `#:package` directives at the top of the file, so it can run independently of the `.csproj`.

Because a `.csproj` also exists in this directory, you must pass `--file` explicitly so `dotnet run` treats it as a file-based app instead of an argument to the project:

```powershell
dotnet run --file FunctionSample.cs
```

This demonstrates giving the model a callable tool (`GetTicketStatus`) via `Microsoft.Extensions.AI`'s `IChatClient`, with `UseFunctionInvocation()` enabling automatic tool-call-and-continue behavior.

> Note: `dotnet build FunctionSample.cs` or `dotnet run FunctionSample.cs` (without `--file`) will **not** work as expected here — the first ignores the file's `#:package` directives, and the second gets treated as a command-line argument to the project instead of the file itself.

## Required NuGet packages

The main project ([MaiThinkingDemo.csproj](MaiThinkingDemo.csproj)) references:

| Package | Version | Purpose |
| --- | --- | --- |
| `Azure.AI.OpenAI` | `2.9.0-beta.1` | Azure OpenAI client (`AzureOpenAIClient`, `GetChatClient`) |
| `Azure.Identity` | `1.21.0` | `DefaultAzureCredential` authentication |
| `Microsoft.Extensions.AI` | `10.9.0` | `IChatClient`, `ChatMessage`, `ChatOptions`, `AIFunctionFactory` abstractions |
| `Microsoft.Extensions.AI.OpenAI` | `10.3.0` | `AsIChatClient()` extension for the OpenAI `ChatClient` |
| `Microsoft.Extensions.Configuration` | `10.0.0` | Configuration builder |
| `Microsoft.Extensions.Configuration.EnvironmentVariables` | `10.0.0` | Environment variable configuration source |
| `Microsoft.Extensions.Configuration.UserSecrets` | `10.0.0` | User secrets configuration source |

> `Microsoft.Extensions.AI.AzureAIInference` is also present in the `.csproj` but is not currently used by either sample.

Restore them with:

```powershell
dotnet restore
```

`FunctionSample.cs` declares the same packages itself via `#:package` directives at the top of the file (see [Running the standalone sample](#running-the-standalone-sample-functionsamplecs)), so it resolves its own dependencies when run with `--file` and does not need `dotnet restore` run against it separately.

### Version pinning note

`Azure.AI.OpenAI@2.1.0` (the latest stable release) is compiled against `OpenAI@2.1.0` and breaks at runtime (`MissingMethodException`) if a newer transitive `OpenAI` package version is resolved — which happens when `Microsoft.Extensions.AI.OpenAI` pulls in a newer `OpenAI` version. Use `Azure.AI.OpenAI@2.9.0-beta.1` (or newer versions verified compatible with your `Microsoft.Extensions.AI.OpenAI` version) to avoid this.

## Project structure

```
Program.cs                    # Main project entry point (contract risk analysis)
FunctionSample.cs             # Standalone file-based app (IChatClient + tool calling)
MaiThinkingDemo.csproj
CONTRIBUTING.md               # How to contribute
.github/workflows/dotnet.yml  # Build workflow
LICENSE                       # MIT license
```

## License

This project is licensed under the MIT License. See [LICENSE](LICENSE).
