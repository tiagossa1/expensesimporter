namespace Application;

public static class ChoicesConstants
{
    public readonly static IReadOnlyList<string> Choices = new[] {
        ActivoBank,
        Leave
    }.AsReadOnly();

    public const string Leave = "LEAVE";

    public const string ActivoBank = "ACTIVOBANK";
}
