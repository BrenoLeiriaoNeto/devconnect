namespace DevConnect.Application.Contracts.Models;

public class ApiResponseModel<T>
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    
    public static ApiResponseModel<T> Ok(T data) => 
        new() {Success = true, StatusCode = 200, Data = data};
    
    public static ApiResponseModel<T> Fail(string message, int statusCode = 500) => 
        new() {Success = false, StatusCode = statusCode, Message = message};
}