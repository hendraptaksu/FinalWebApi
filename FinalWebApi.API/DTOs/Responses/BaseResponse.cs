namespace FinalWebApi.API.DTOs.Responses;

public class BaseResponse<T>
{
    public T Data { get; set; }
    public string Message { get; set; }
}