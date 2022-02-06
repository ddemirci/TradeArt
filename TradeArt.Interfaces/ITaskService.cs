using TradeArt.Core;

namespace TradeArt.Interfaces
{
    public interface ITaskService
    {
        string InvertText(string text);
        Task<bool> FunctionA();
        Result<string> CalculateSHA256Hash(string filePath);
    }
}