namespace Core.Entities
{
    public class ApiCallLog
    {
        public int Id { get; set; }
        public string? Endpoint { get; set; }
        public string? Request { get; set; }
        public string? Response { get; set; }
        public DateTime Timestamp { get; set; }
        public bool Success { get; set; }
    }
}
