namespace WebApplication1.Common
{
    public class ServiceResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;

        public static ServiceResult Success(string message = "Success")
        {
            return new ServiceResult { IsSuccess = true, Message = message };
        }

        public static ServiceResult Error(string message)
        {
            return new ServiceResult { IsSuccess = false, Message = message };
        }
    }

    // Əgər məlumat daşıyan generic versiyası lazımdırsa:
    public class ServiceResult<T> : ServiceResult
    {
        public T? Data { get; set; }

        public static ServiceResult<T> Success(T data, string message = "Success")
        {
            return new ServiceResult<T> { IsSuccess = true, Data = data, Message = message };
        }

        new public static ServiceResult<T> Error(string message)
        {
            return new ServiceResult<T> { IsSuccess = false, Message = message };
        }
    }
}
