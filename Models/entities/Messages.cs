namespace Models.entities
{
    public class Messages
    {
        public long Id { get; set; }
        public long Client_Id { get; set; }
        public string? Message { get; set; }
        public string? Topic { get; set; }
        public int StatusCode { get; set; }
        public int? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public string? ErrorType { get; set;}
        public bool IsReceived { get; set; }
        public bool IsRead { get; set; }
        public DateTime Date { get; set; }
    }
}
