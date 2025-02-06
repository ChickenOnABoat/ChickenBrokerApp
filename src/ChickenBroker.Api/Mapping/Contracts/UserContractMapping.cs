using ChickenBroker.Application.Models;
using ChickenBroker.Contracts.Requests.User;
using ChickenBroker.Contracts.Responses.User;

namespace ChickenBroker.Api.Mapping.Contracts;

public static class UserContractMapping
{
    public static User MapToUser(this CreateUserRequest request)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            CurrentPropertyAgencyId = request.CurrentPropertyAgencyId
        };
    }

    public static UserResponse MapToResponse(this User request)
    {
        return new UserResponse
        {
            Id = request.Id,
            Name = request.Name,
            Email = request.Email,
            CurrentPropertyAgencyId = request.CurrentPropertyAgencyId
        };
    }

    public static IEnumerable<UserResponse> MapToResponse(this IEnumerable<User> requests)
    {
        return requests.Select(MapToResponse);
    }
}