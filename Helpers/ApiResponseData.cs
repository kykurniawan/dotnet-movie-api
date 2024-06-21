using System.Net;
using MovieApi.Http.Responses;

namespace MovieApi.Helpers;

public class ApiResponseData
{
    public static ApiResponse<T> Create<T>(HttpStatusCode statusCode, T? data, string message, object? errors = null)
    {
        return new ApiResponse<T>
        {
            StatusCode = (int)statusCode,
            Data = data,
            Message = message,
            Errors = errors
        };
    }
}