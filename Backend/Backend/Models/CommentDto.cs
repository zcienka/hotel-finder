﻿namespace Backend.Models
{
    public class CommentDto
    {
        public required string Description { get; set; }
        public required string UserEmail { get; set; }
        public required string HotelId { get; set; }
    }
}