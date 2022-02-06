namespace TradeArt.Core
{
    public class Result<T>
    {
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }

        private Result(T data)
        {
            Data = data;
            IsSuccess = true;
        }  

        private Result(string errorMessage)
        {
            IsSuccess = false;
            ErrorMessage = errorMessage;
        }

        public static Result<T> AsSuccess(T data)
        {
            return new Result<T>(data);
        }

        public static Result<T> AsFailure(string errorMessage)
        {
            return new Result<T>(errorMessage);
        }
    }
}