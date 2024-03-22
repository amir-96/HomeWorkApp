namespace Domain.BaseModels
{
    public class ServerResponse<T>
    {
        public bool IsSucceeded { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ServerResponse(bool status, string message, T data)
        {
            IsSucceeded = status;
            Message = message;
            Data = data;
        }
    }
}
