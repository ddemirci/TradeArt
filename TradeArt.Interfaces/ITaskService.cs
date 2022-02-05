namespace TradeArt.Interfaces
{
    public interface ITaskService
    {
        string InvertText(string text);
        Task FunctionA();
        string CalculateSHA256Hash(string filePath);
    }
}