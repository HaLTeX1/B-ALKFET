using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookFlow.Api.Models;

public class Book
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("title")]
    public string Title { get; set; } = string.Empty;

    [BsonElement("author")]
    public string Author { get; set; } = string.Empty;

    [BsonElement("genre")]
    public string Genre { get; set; } = string.Empty;

    [BsonElement("publishedYear")]
    public int PublishedYear { get; set; }

    [BsonElement("availableCopies")]
    public int AvailableCopies { get; set; }

    [BsonElement("createdAtUtc")]
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}