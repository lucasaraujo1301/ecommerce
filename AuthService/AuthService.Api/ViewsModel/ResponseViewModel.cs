namespace ECommerce.AuthService.Api.ViewsModel
{
    public class ResponseViewModel<T>
    {
        public bool IsSuccess { get; set; } = true;
        public Dictionary<string, object>? Errors { get; set; }
        public T? Data { get; set; }

        public static ResponseViewModel<T> Success(T data) => new() {Data=data};
        public static ResponseViewModel<T> Failure(Dictionary<string, object> errors) => new() { IsSuccess = false, Errors = errors };
    }
}