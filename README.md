# Expenses Importer

This lightweight tool efficiently converts bank expenses report into CSV format, tailored specifically for seamless integration with the popular "[Expenses](https://getexpenses.app/)" app. With just a few simple steps, you can transform complex financial into a clean and structured CSV format compatible with the Expenses app, streamlining your expense management process.

## Compatibility

### [ActivoBank](https://www.activobank.pt/pt/)

Activobank is a modern banking solution offering innovative financial services tailored to meet the diverse needs of today's customers. With a focus on digital convenience and personalized support, Activobank provides a seamless banking experience that empowers individuals and businesses to manage their finances with ease.

## AppSettings

### CategoryMappers

This is used to accurately categorise an expense. It's a basic dictionary in which the key is a portion of (or the entire) description and the value is the Expense category. Remember that the Expense category must be **exactly** the name of the category.

## Setting Up

Clone the repository

```bash
git clone https://github.com/tiagossa1/expensesimporter.git
```

Edit the example appsettings.json

Run the application

```sh
dotnet run
```

## Contribution Guidelines

Contributions are welcome! If you have any ideas for improvements or new features, feel free to submit a pull request. Please adhere to the existing code style and ensure comprehensive test coverage for any changes.
