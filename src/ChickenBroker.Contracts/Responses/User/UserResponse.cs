namespace ChickenBroker.Contracts.Responses.User;

public class UserResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public Guid? CurrentPropertyAgencyId { get; set; } 
}