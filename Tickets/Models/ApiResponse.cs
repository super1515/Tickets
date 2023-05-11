namespace Tickets.Models
{
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public static ApiResponse<T> Fail(string errorMessage)
        {
            return new ApiResponse<T> { Succeeded = false, Message = errorMessage };
        }
        public static ApiResponse<T> Success(T? data, string message)
        {
            return new ApiResponse<T> { Succeeded = true, Data = data, Message = message };
        }
    }
}
