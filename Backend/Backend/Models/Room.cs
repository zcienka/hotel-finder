﻿namespace Backend.Models
{
    public class Room
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public required int Capacity { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required float Price { get; set; }
        public required string HotelId { get; set; }
    }
}
