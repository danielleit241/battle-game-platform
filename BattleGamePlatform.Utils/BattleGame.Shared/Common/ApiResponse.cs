namespace BattleGame.Shared.Common
{
    public class ApiResponse<T> where T : class
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public object? Metadata { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, string message = "Success", object? metadata = null)
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                Message = message,
                Data = data,
                Metadata = metadata
            };
        }

        public static ApiResponse<T> FailureResponse(string message, object? metadata = null)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = message,
                Data = default,
                Metadata = metadata
            };
        }
    }
}
