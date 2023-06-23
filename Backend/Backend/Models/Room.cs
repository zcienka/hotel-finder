namespace Backend.Models
{
    public class Room
    {
        public int Id { get; set; }
        public required int Capacity { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required int Price { get; set; }
    }
}
