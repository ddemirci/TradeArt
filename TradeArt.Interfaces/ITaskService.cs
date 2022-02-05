namespace TradeArt.Interfaces
{
    public interface ITaskService
    {
        string InvertText(string text);
        Task<bool> FunctionA();
        string CalculateSHA256Hash(string filePath);
    }
}