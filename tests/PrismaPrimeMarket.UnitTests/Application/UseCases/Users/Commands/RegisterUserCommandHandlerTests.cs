using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using PrismaPrimeMarket.Application.DTOs.User;
using PrismaPrimeMarket.Application.Mappings;
using PrismaPrimeMarket.Application.UseCases.Users.Commands.RegisterUser;
using PrismaPrimeMarket.Domain.Entities;
using PrismaPrimeMarket.Domain.Exceptions;
using PrismaPrimeMarket.Domain.Interfaces;
using PrismaPrimeMarket.Domain.Interfaces.Repositories;

namespace PrismaPrimeMarket.UnitTests.Application.UseCases.Users.Commands;

public class RegisterUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IMapper _mapper;
    private readonly RegisterUserCommandHandler _handler;

    public RegisterUserCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        // Setup UserManager mock
        var userStoreMock = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(
            userStoreMock.Object, null, null, null, null, null, null, null, null);

        // Setup AutoMapper
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<UserProfile>();
        });
        _mapper = config.CreateMapper();

        _handler = new RegisterUserCommandHandler(
            _userRepositoryMock.Object,
            _userManagerMock.Object,
            _unitOfWorkMock.Object,
            _mapper
        );
    }

    [Fact]
    public async Task Handle_WithValidData_ShouldCreateUser()
    {
        // Arrange
        var command = new RegisterUserCommand(
            "johndoe",
            "John",
            "SecurePass123!"
        );

        _userRepositoryMock.Setup(x => x.ExistsByUserNameAsync(command.UserName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), command.Password))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.UserName.Should().Be(command.UserName);
        result.FirstName.Should().Be(command.FirstName);

        _userRepositoryMock.Verify(x => x.ExistsByUserNameAsync(command.UserName, It.IsAny<CancellationToken>()), Times.Once);
        _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<User>(), command.Password), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithExistingUserName_ShouldThrowUserAlreadyExistsException()
    {
        // Arrange
        var command = new RegisterUserCommand(
            "johndoe",
            "John",
            "SecurePass123!"
        );

        _userRepositoryMock.Setup(x => x.ExistsByUserNameAsync(command.UserName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UserAlreadyExistsException>()
            .WithMessage("*nome de usuário*");

        _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithExistingEmail_ShouldThrowUserAlreadyExistsException()
    {
        // Arrange
        var command = new RegisterUserCommand(
            "johndoe",
            "John",
            "SecurePass123!",
            Email: "john@example.com"
        );

        _userRepositoryMock.Setup(x => x.ExistsByUserNameAsync(command.UserName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _userRepositoryMock.Setup(x => x.ExistsByEmailAsync(command.Email!, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UserAlreadyExistsException>()
            .WithMessage("*e-mail*");

        _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithFailedIdentityResult_ShouldThrowDomainException()
    {
        // Arrange
        var command = new RegisterUserCommand(
            "johndoe",
            "John",
            "SecurePass123!"
        );

        _userRepositoryMock.Setup(x => x.ExistsByUserNameAsync(command.UserName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var identityErrors = new[]
        {
            new IdentityError { Code = "PasswordTooShort", Description = "Password is too short" }
        };

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), command.Password))
            .ReturnsAsync(IdentityResult.Failed(identityErrors));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("*Erro ao criar usuário*");

        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
