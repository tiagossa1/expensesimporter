using Application;
using Application.Converters;
using Application.Interfaces;
using FluentResults;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

var serviceProvider = new ServiceCollection()
    .AddLogging()
    .AddSingleton<IConverter<Result<ActivoBankResponse>>, ActivoBankConverter>()
    .BuildServiceProvider();

var choice = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("What converter do you want to use?")
        .AddChoices(ChoicesConstants.Choices));

if (ChoicesConstants.Leave.Equals(choice, StringComparison.InvariantCultureIgnoreCase))
{
    Environment.Exit(0);
}
else if (ChoicesConstants.ActivoBank.Equals(choice, StringComparison.InvariantCultureIgnoreCase))
{
    var service = serviceProvider.GetService<IConverter<Result<ActivoBankResponse>>>() ?? throw new NotImplementedException($"Service for {choice} is not implemented");

    var filePath = AnsiConsole.Ask<string>("Specify the CSV file: ");

    ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

    var result = await service.Convert(filePath.Trim());

    if (result.IsSuccess)
    {
        AnsiConsole.Markup($"[green]Expense compatible CSV has been generated in '{result.Value.Path}'.[/]");
    }
    else
    {
        AnsiConsole.Write($"Expense compatible CSV could not be generated: '{string.Join(",", result.Errors.Select(e => e.Message))}'");
    }
}
else
{
    throw new NotSupportedException($"{choice} is not supported");
}