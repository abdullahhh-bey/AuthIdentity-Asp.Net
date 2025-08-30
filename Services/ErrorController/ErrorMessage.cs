namespace UserAuthManagement.Services.ErrorController
{
    public class ErrorMessage
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Details { get; set; }
    }
}
