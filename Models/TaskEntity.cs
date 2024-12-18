using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;


namespace TaskService.Models;

public class TaskEntity
{
 	[BsonId]
    [BsonRepresentation(BsonType.String)] 
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Status { get; set; } = "Pending"; 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
