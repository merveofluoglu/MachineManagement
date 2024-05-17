namespace Models.entities
{
    public class Machines
    {
        public long Id { get; set; }
        public required string MachineName { get; set; }
        public string? Description { get; set; }
        public required string Status { get; set; }
        public int MessageCount { get; set; }
    }
}
