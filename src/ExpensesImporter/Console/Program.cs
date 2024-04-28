using Application;
using Application.Converters;
using Application.Interfaces;
using FluentResults;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Spectre.Console;

var configuration = new ConfigurationBuilder()
  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
  .Build();

var serviceProvider = new ServiceCollection()
    .Configure<Dictionary<string, string>>(configuration.GetSection("CategoryMappers"))
    .AddLogging(loggingBuilder =>
    {
        loggingBuilder.AddConsole();
        loggingBuilder.SetMinimumLevel(LogLevel.Information);
    })
    .AddSingleton<ICsvReader, CsvReader>()
    .AddSingleton<ICsvWriter, CsvWriter>()
    .AddSingleton<IConverter<Result<ActivoBankResponse>>, ActivoBankConverter>()
    .AddSingleton<ICategoryMapper, CategoryMapper>()
    .AddSingleton<IFilePathValidator, ActivoBankFilePathValidator>()
    .BuildServiceProvider();

var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

await HandleSelectedChoice(serviceProvider, logger);

static string GetFilePath(IFilePathValidator filePathService)
{
    var filePath = "";
    var isValid = false;

    do
    {
        filePath = AnsiConsole.Ask<string>("Please, specify the CSV file: ");
        isValid = filePathService.IsValid(filePath);

        if (!isValid)
        {
            AnsiConsole.MarkupLine($"[red]{filePath} is not valid.[/]");
        }
    } while (!isValid);

    return filePath;
}

static async Task HandleSelectedChoice(ServiceProvider serviceProvider, ILogger<Program> logger)
{
    var choice = "";

    do
    {
        choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("What converter do you want to use?")
            .AddChoices(ChoicesConstants.Choices));

        logger.LogInformation("User picked '{choice}'", choice);

        if (ChoicesConstants.Leave.Equals(choice, StringComparison.InvariantCultureIgnoreCase))
        {
            Environment.Exit(0);
        }
        else if (ChoicesConstants.ActivoBank.Equals(choice, StringComparison.InvariantCultureIgnoreCase))
        {
            var service = serviceProvider.GetService<IConverter<Result<ActivoBankResponse>>>() ?? throw new NotImplementedException($"Service for {choice} is not implemented");

            var filePathService = serviceProvider.GetServices<IFilePathValidator>()
                ?.FirstOrDefault(filePathService => nameof(ActivoBankFilePathValidator).Equals(filePathService.Name, StringComparison.InvariantCultureIgnoreCase));

            ArgumentNullException.ThrowIfNull(filePathService, "Cannot get required service to check if file path is valid");

            AnsiConsole.WriteLine("For this to work, you will need to convert ActivoBank's XLSL file to CSV so that it can be easily manipulated by the converter.");

            string filePath = GetFilePath(filePathService);

            var result = await service.Convert(filePath.Trim());

            if (result is null)
            {
                AnsiConsole.WriteLine($"Expense compatible CSV could not be generated");
            }
            else if (result.IsSuccess)
            {
                AnsiConsole.MarkupLine($"[green]Expense compatible CSV has been generated in '{result.Value.Path}'.[/]");
            }
            else
            {
                AnsiConsole.WriteLine($"Expense compatible CSV could not be generated: '{string.Join(",", result.Errors.Select(e => e.Message))}'");
            }
        }
        else
        {
            throw new NotSupportedException($"{choice} is not supported");
        }

        AnsiConsole.MarkupLine("[green]The console screen will be cleared in 3 seconds...[/]");

        Thread.Sleep(3000);
        Console.Clear();
    } while (!ChoicesConstants.Leave.Equals(choice, StringComparison.InvariantCultureIgnoreCase));
}