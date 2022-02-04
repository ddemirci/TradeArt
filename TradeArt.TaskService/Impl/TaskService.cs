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

        public async Task FunctionA()
        {
            var list = new List<Task<bool>>();
            for (int i = 1; i < 1001; i++)
            {
                list.Add(Task.Run(() => FunctionB(i)));
            }
            await Task.WhenAll(list);
        }

        private async Task<bool> FunctionB(int data)
        {
            await Task.Delay(100); //To behave as processing data that comes from A
            return true;
        }

    }
}
