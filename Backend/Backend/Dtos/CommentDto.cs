﻿namespace Backend.Dtos
{
    public class CommentDto
    {
        public required string Description { get; set; }
        public required string UserId { get; set; }
        public required string HotelId { get; set; }
    }
}