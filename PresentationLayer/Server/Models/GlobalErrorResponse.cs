namespace Server.Models
{
    public class GlobalErrorResponse
    {
        public int StatusCode { get; set; }
        public string? Title { get; set; }
        public string? ErrorMessage { get; set; }

    }
}
