using Application.DTOs.Response;
using Application.Features.Users.Queries.GetUserByWorkshopId;
using MediatR;
using System.Text.Json;
using Xunit;

namespace API.Tests;

public class UserDtoContractTests
{
    [Fact]
    public void User_dto_does_not_expose_password_hash()
    {
        Assert.Null(typeof(UserDTO).GetProperty("PasswordHash"));

        var json = JsonSerializer.Serialize(new UserDTO
        {
            Id = Guid.NewGuid(),
            Role = "QC",
            FullName = "Test User",
            Email = "test@example.invalid"
        });

        Assert.DoesNotContain("PasswordHash", json, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Workshop_lookup_query_targets_user_dto()
    {
        Assert.True(typeof(IRequest<UserDTO>).IsAssignableFrom(typeof(GetUserByWorkshopIdQuery)));
    }
}
