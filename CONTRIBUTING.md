# Contributing

Thanks for helping improve this sample. It's a small .NET 10 project for MAI-Thinking-1 that uses the `Microsoft.Extensions.AI` `IChatClient` abstraction with Entra ID: a contract risk analysis in `Program.cs`, and a tool-calling example in the file-based app `FunctionSample.cs`. Contributions that make it clearer, more correct, or more useful to other .NET developers are welcome.

## Ways to contribute

- **Report a bug.** A sample fails, or the README doesn't match what the code does.
- **Fix the docs.** Typos, unclear steps, or a missing troubleshooting case (for example, a package version that breaks at runtime).
- **Improve the samples.** For example, streaming the reasoning output, or a second tool in `FunctionSample.cs`.
- **Share results.** If you ran the contract analysis on your own clauses, an issue describing what the model caught helps others.

For anything larger than a small fix, open an issue first so we can agree on the approach before you write code.

## Set up

1. Install the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) and the Azure CLI.
2. Clone the repo and build it:

   ```bash
   git clone https://github.com/taswar/MaiThinkingDemo.git
   cd MaiThinkingDemo
   dotnet build
   ```

3. To run it, deploy MAI-Thinking-1, set `AZURE_AI_ENDPOINT` (and `AZURE_OPENAI_DEPLOYMENT` if your deployment isn't named `mai-thinking-1`) with user secrets, and sign in with `az login`, as described in the [README](README.md#configuration). Run the tool-calling sample with `dotnet run --file FunctionSample.cs`.

## Guidelines

- **Never commit secrets.** No keys, endpoints, tenant IDs or tokens in code, config files or screenshots. Settings belong in user secrets.
- **Keep Entra ID.** Both samples use `DefaultAzureCredential` with no API key. Keep it that way.
- **Keep `IChatClient`.** Both samples go through `Microsoft.Extensions.AI`, so the same code works with other providers. Don't call the OpenAI client directly.
- **Keep the two samples in step.** `FunctionSample.cs` declares its packages with `#:package`. If you change a package version in the `.csproj`, change it in `FunctionSample.cs` too, and keep `FunctionSample.cs` excluded from the project.
- **Mind the package versions.** `Azure.AI.OpenAI` 2.1.0 breaks at runtime with newer `OpenAI` packages. Test any version change with both samples.
- **Keep tools fake and read-only.** `GetTicketStatus` returns made-up data. Don't connect a tool to a real system.
- **Keep it a sample.** The code should stay short and readable. Prefer clarity over abstraction, and add a dependency only when it clearly helps.
- **Keep the README in sync.** If you change settings, packages or output, update the README in the same pull request.
- **One change per pull request.** Small, focused pull requests are easier to review and merge.

## Pull request checklist

- [ ] `dotnet build` succeeds with no new warnings.
- [ ] `dotnet run` and, if you changed it, `dotnet run --file FunctionSample.cs` work against your own deployment, or you explain why you couldn't run them.
- [ ] No secrets, endpoints or personal data are in the diff.
- [ ] The README reflects any change in settings, packages or output.
- [ ] The pull request description says what changed and why.

The build workflow runs on every pull request and has to pass before a merge.

## Reporting a bug

Open an issue and include:

- Which sample you ran and what you expected.
- The full error message, with any endpoints removed. For `MissingMethodException`, include the package versions you had.
- Your .NET SDK version (`dotnet --version`) and operating system.
- Your deployment name.

## Security issues

Please don't report security problems in a public issue. Use **Security → Report a vulnerability** on the repo to send a private report.

## License

By contributing, you agree that your contributions are licensed under the [MIT License](LICENSE) that covers this project.
