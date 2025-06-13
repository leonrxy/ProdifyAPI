namespace Prodify.Common
{
    public static class ResponseFactory
    {
        public static ApiResponse<T> Success<T>(T? data, string message = "Success")
            => new ApiResponse<T>
            {
                Status = "success",
                Message = message,
                Data = data
            };

        public static ApiResponse<object> Success(string message)
            => Success<object>(null, message);

        public static ApiResponse<object> Error(string message, string status = "error")
            => new ApiResponse<object>
            {
                Status = status,
                Message = message,
                Data = null
            };
    }
}
