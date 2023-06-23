﻿namespace Backend.Models
{
    public class HotelDto
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public required string PhoneNumber { get; set; }
    }
}