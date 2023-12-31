﻿namespace Backend.Models;
public class Hotel
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Address { get; set; }
    public required string City { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Category { get; set; }
    public int Stars { get; set; } = 0;

    public virtual ICollection<Room> Rooms { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
    public virtual ICollection<Reservation> Reservations { get; set; }
}
