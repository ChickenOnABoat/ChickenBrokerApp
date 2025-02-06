namespace ChickenBroker.Contracts.Requests.User;

public class CreateUserRequest
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public Guid? CurrentPropertyAgencyId { get; set; }
}