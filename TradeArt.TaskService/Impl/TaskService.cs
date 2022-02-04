using TradeArt.Interfaces;

namespace TradeArt.TaskService.Impl
{
    public class TaskService : ITaskService
    {
        public string InvertText(string text)
        {
            if (text.Length <= 1) return text;

            var arr = text.ToCharArray();
            Array.Reverse(arr);
            return string.Join("", arr);
        }

    }
}
