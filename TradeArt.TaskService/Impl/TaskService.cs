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

        public async Task<bool> FunctionA()
        {
            var list = new List<Task<bool>>();
            for (int i = 1; i < 1001; i++)
            {
                list.Add(Task.Run(() => FunctionB(i)));
            }
            await Task.WhenAll(list);
            return list.All(x => x.Status == TaskStatus.RanToCompletion);
        }

        private async Task<bool> FunctionB(int data)
        {
            await Task.Delay(100); //To behave as processing data that comes from A
            return true;
        }

    }
}
