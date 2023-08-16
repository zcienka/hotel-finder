namespace Backend.Dtos;
public class CommentDto
{
    public string? Id { get; set; }
    public required string Description { get; set; }
    public required string UserEmail { get; set; }
    public required string HotelId { get; set; }
}
