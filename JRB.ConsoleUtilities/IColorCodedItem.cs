namespace JRB.ConsoleUtilities
{
    public interface IColorCodedItem
    {
        ConsoleColor OutputColor { get; }

        string GetInitialDisplayText();
        string GetSubsequentDisplayText();
    }
}
