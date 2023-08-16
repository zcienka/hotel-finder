﻿namespace Backend.Requests
{
    public class RoomRequest
    {
        public string? Id { get; set; }
        public required int Capacity { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required float Price { get; set; }
        public required string HotelId { get; set; }
    }
}
