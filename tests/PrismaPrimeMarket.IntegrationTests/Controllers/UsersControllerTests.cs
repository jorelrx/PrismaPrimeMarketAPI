using System.Net;
using System.Net.Http.Json;
using Bogus;
using FluentAssertions;
using PrismaPrimeMarket.Application.Common.Models;
using PrismaPrimeMarket.Application.DTOs.User;
using PrismaPrimeMarket.IntegrationTests.Setup;

namespace PrismaPrimeMarket.IntegrationTests.Controllers;

public class UsersControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Faker _faker;

    public UsersControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _faker = new Faker("pt_BR");
    }

    [Fact]
    public async Task Register_WithValidData_ShouldReturnCreated()
    {
        // Arrange
        var createUserDto = new CreateUserDto
        {
            UserName = _faker.Internet.UserName(),
            FirstName = _faker.Name.FirstName(),
            Password = "SecurePass123!",
            ConfirmPassword = "SecurePass123!"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/Users", createUserDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var result = await response.Content.ReadFromJsonAsync<Response<UserDto>>();
        result.Should().NotBeNull();
        result!.Succeeded.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.UserName.Should().Be(createUserDto.UserName);
        result.Data.FirstName.Should().Be(createUserDto.FirstName);
    }

    [Fact]
    public async Task Register_WithoutRequiredFields_ShouldReturnBadRequest()
    {
        // Arrange
        var createUserDto = new CreateUserDto
        {
            UserName = "",
            FirstName = "",
            Password = "",
            ConfirmPassword = ""
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/Users", createUserDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var result = await response.Content.ReadFromJsonAsync<Response<string>>();
        result.Should().NotBeNull();
        result!.Succeeded.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Register_WithExistingUserName_ShouldReturnBadRequest()
    {
        // Arrange
        var userName = _faker.Internet.UserName();
        var firstUser = new CreateUserDto
        {
            UserName = userName,
            FirstName = _faker.Name.FirstName(),
            Password = "SecurePass123!",
            ConfirmPassword = "SecurePass123!"
        };

        await _client.PostAsJsonAsync("/api/v1/Users", firstUser);

        var duplicateUser = new CreateUserDto
        {
            UserName = userName,
            FirstName = _faker.Name.FirstName(),
            Password = "SecurePass123!",
            ConfirmPassword = "SecurePass123!"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/Users", duplicateUser);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetAll_ShouldReturnListOfUsers()
    {
        // Arrange
        var createUserDto = new CreateUserDto
        {
            UserName = _faker.Internet.UserName(),
            FirstName = _faker.Name.FirstName(),
            Password = "SecurePass123!",
            ConfirmPassword = "SecurePass123!"
        };

        await _client.PostAsJsonAsync("/api/v1/Users", createUserDto);

        // Act
        var response = await _client.GetAsync("/api/v1/Users");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<Response<List<UserDto>>>();
        result.Should().NotBeNull();
        result!.Succeeded.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetById_WithExistingUser_ShouldReturnUser()
    {
        // Arrange
        var createUserDto = new CreateUserDto
        {
            UserName = _faker.Internet.UserName(),
            FirstName = _faker.Name.FirstName(),
            Password = "SecurePass123!",
            ConfirmPassword = "SecurePass123!"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/v1/Users", createUserDto);
        var createResult = await createResponse.Content.ReadFromJsonAsync<Response<UserDto>>();
        var userId = createResult!.Data!.Id;

        // Act
        var response = await _client.GetAsync($"/api/v1/Users/{userId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<Response<UserDto>>();
        result.Should().NotBeNull();
        result!.Succeeded.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Id.Should().Be(userId);
    }

    [Fact]
    public async Task GetById_WithNonExistingUser_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/v1/Users/{nonExistingId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateProfile_WithValidData_ShouldReturnOk()
    {
        // Arrange
        var createUserDto = new CreateUserDto
        {
            UserName = _faker.Internet.UserName(),
            FirstName = _faker.Name.FirstName(),
            Password = "SecurePass123!",
            ConfirmPassword = "SecurePass123!"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/v1/Users", createUserDto);
        var createResult = await createResponse.Content.ReadFromJsonAsync<Response<UserDto>>();
        var userId = createResult!.Data!.Id;

        var updateDto = new UpdateUserProfileDto
        {
            FirstName = "UpdatedName",
            LastName = "UpdatedLastName"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/v1/Users/{userId}/profile", updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<Response<UserDto>>();
        result.Should().NotBeNull();
        result!.Succeeded.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.FirstName.Should().Be(updateDto.FirstName);
        result.Data.LastName.Should().Be(updateDto.LastName);
    }
}
