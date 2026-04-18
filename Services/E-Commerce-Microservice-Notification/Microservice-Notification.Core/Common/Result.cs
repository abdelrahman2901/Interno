using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice_Notification.Core.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public T? Data { get; set; }
        public int StatusCode { get; set; } = 200;

        public static Result<T> Success(T data) => new() { IsSuccess = true, Data = data };
        public static Result<T> NotFound(string message) => new() { IsSuccess = false, ErrorMessage = message, StatusCode = 404 };
        public static Result<T> BadRequest(string message) => new() { IsSuccess = false, ErrorMessage = message, StatusCode = 400 };
        public static Result<T> InternalError(string message) => new() { IsSuccess = false, ErrorMessage = message, StatusCode = 500 };
    }
}
