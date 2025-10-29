using BattleGame.MessageBus.Events;
using BattleGame.UserService.BusinessLogicLayer.Services.Abstractions;
using BattleGame.UserService.BusinessLogicLayer.Services.Implementations;
using BattleGame.UserService.Common.Dtos;
using BattleGame.UserService.Common.Entities;
using BattleGame.UserService.DataAccessLayer.Repositories.Abstractions;
using FluentAssertions;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Linq.Expressions;


namespace BattleGame.UnitTests
{
    public class UserServicesTests
    {
        private readonly Mock<IUserRepository> _userRepoMock = new();
        private readonly Mock<IRoleRepository> _roleRepoMock = new();
        private readonly Mock<ITokenServices> _tokenServiceMock = new();
        private readonly Mock<IPublishEndpoint> _publishMock = new();

        private readonly UserServices _service;

        public UserServicesTests()
        {
            _service = new UserServices(
                _userRepoMock.Object,
                _roleRepoMock.Object,
                _tokenServiceMock.Object,
                _publishMock.Object
            );
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnSuccess_WhenUsersExist()
        {
            var users = new List<User>
            {
                new() {
                    Id = Guid.NewGuid(),
                    Username = "test",
                    Email = "test@example.com",
                    Role = new Role{
                        Id = Guid.NewGuid(),
                        Name = "PLAYER"
                    }
                }
            };

            _userRepoMock.Setup(r => r.GetAllUserIncludeRoleAsync())
                         .Returns(Task.FromResult<IReadOnlyCollection<User>>(users));

            var result = await _service.GetAllUsersAsync();

            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeEmpty();
            result.Data.Should().HaveCount(1);
            result.Message.Should().Be("Users retrieved successfully");
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnFailure_WhenNoUsersExist()
        {
            _userRepoMock.Setup(r => r.GetAllUserIncludeRoleAsync())
                         .Returns(Task.FromResult<IReadOnlyCollection<User>>(null!));

            var result = await _service.GetAllUsersAsync();

            result.IsSuccess.Should().BeFalse();
            result.Data.Should().BeNull();
            result.Message.Should().Be("No users found");
        }

        [Fact]
        public async Task GetUserById_ShouldReturnSuccess_WhenUserExist()
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Username = "test",
                Email = "test@example.com",
                Role = new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "PLAYER"
                }
            };

            _userRepoMock.Setup(r => r.GetUserIncludeRoleAsync(user.Id))
                .ReturnsAsync((User)user);

            var response = await _service.GetUserByIdAsync(user.Id);

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.Data.Should().NotBeNull();
            response.Message.Should().Be("User retrieved successfully");
        }

        [Fact]
        public async Task GetUserById_ShouldReturnFailure_WhenNoUserExist()
        {
            var id = Guid.NewGuid();
            _userRepoMock.Setup(r => r.GetUserIncludeRoleAsync(id))
                .ReturnsAsync((User)null!);

            var response = await _service.GetUserByIdAsync(id);

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeFalse();
            response.Data.Should().BeNull();
            response.Message.Should().Be("No users found");
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldReturnSuccess_WhenRoleExist()
        {
            var roleId = Guid.NewGuid();
            var role = new Role
            {
                Id = roleId,
                Name = "PLAYER"
            };
            _roleRepoMock.Setup(r => r.GetAsync(It.IsAny<Expression<Func<Role, bool>>>()))
                         .ReturnsAsync(role);

            _userRepoMock.Setup(r => r.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                           .ReturnsAsync((User?)null);

            _userRepoMock.Setup(r => r.AddAsync(It.IsAny<User>()))
                               .ReturnsAsync((User u) =>
                               {
                                   u.Id = Guid.NewGuid();
                                   u.Role = new Role { Id = roleId, Name = "PLAYER" };
                                   return u;
                               });

            _publishMock.Setup(p => p.Publish(It.IsAny<UserCreatedEvent>(), It.IsAny<CancellationToken>()))
                        .Returns(Task.CompletedTask);

            var createUserDto = new CreateUserDto(
                Username: "newuser",
                Email: "",
                Password: "password",
                RoleId: roleId
            );

            var reponseRegister = await _service.RegisterUserAsync(createUserDto);

            reponseRegister.Should().NotBeNull();
            reponseRegister.IsSuccess.Should().BeTrue();
            reponseRegister.Data.Should().NotBeNull();
            reponseRegister.Message.Should().Be("User registered successfully");
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldReturnFailure_WhenNoRoleExist()
        {
            var roleId = Guid.NewGuid();

            _roleRepoMock.Setup(r => r.GetAsync(It.IsAny<Expression<Func<Role, bool>>>()))
                         .ReturnsAsync((Role)null!);

            var createUserDto = new CreateUserDto(
                Username: "newuser",
                Email: "",
                Password: "password",
                RoleId: roleId
            );

            var reponseRegister = await _service.RegisterUserAsync(createUserDto);
            reponseRegister.Should().NotBeNull();
            reponseRegister.IsSuccess.Should().BeFalse();
            reponseRegister.Data.Should().BeNull();
            reponseRegister.Message.Should().Be("Role does not exist");
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldReturnFailure_WhenUsernameExist()
        {
            var roleId = Guid.NewGuid();
            var role = new Role
            {
                Id = roleId,
                Name = "PLAYER"
            };

            _roleRepoMock.Setup(r => r.GetAsync(It.IsAny<Expression<Func<Role, bool>>>()))
                         .ReturnsAsync(role);

            _userRepoMock.SetupSequence(r => r.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                           .ReturnsAsync((User?)null)
                           .ReturnsAsync(new User
                           {
                               Username = "newuser",
                               Role = new Role
                               {
                                   Id = roleId,
                                   Name = "PLAYER"
                               }
                           });

            var createUserDto = new CreateUserDto(
                Username: "newuser",
                Email: "",
                Password: "password",
                RoleId: roleId
            );

            var reponseRegister1 = await _service.RegisterUserAsync(createUserDto);
            reponseRegister1.Should().NotBeNull();
            reponseRegister1.IsSuccess.Should().BeTrue();
            reponseRegister1.Data.Should().NotBeNull();
            reponseRegister1.Message.Should().Be("User registered successfully");


            var reponseRegister2 = await _service.RegisterUserAsync(createUserDto);
            reponseRegister2.Should().NotBeNull();
            reponseRegister2.IsSuccess.Should().BeFalse();
            reponseRegister2.Data.Should().BeNull();
            reponseRegister2.Message.Should().Be("Username already exists");
        }


        [Fact]
        public async Task LoginUserAsync_ShouldReturnSuccess_WhenCredentialsAreValid()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                PasswordHash = new PasswordHasher<User>().HashPassword(null!, "password"),
                Role = new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "PLAYER"
                }
            };
            _userRepoMock.Setup(r => r.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                         .ReturnsAsync(user);
            _userRepoMock.Setup(r => r.GetUserIncludeRoleAsync(user.Id))
                         .ReturnsAsync(user);
            _tokenServiceMock.Setup(t => t.GenerateAccessToken(It.IsAny<User>()))
                             .Returns("mocked_token");
            var loginDto = new LoginDto(
                Username: "testuser",
                Password: "password"
            );
            var response = await _service.LoginUserAsync(loginDto);
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.Data.Should().NotBeNull();
            response.Data!.Token.Should().Be("mocked_token");
            response.Message.Should().Be("Login successful");
        }

        [Fact]
        public async Task LoginUserAsync_ShouldReturnFailure_WhenUsernameDoesNotExist()
        {
            _userRepoMock.Setup(r => r.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                         .ReturnsAsync((User?)null);
            var loginDto = new LoginDto(
                Username: "nonexistentuser",
                Password: "password"
            );
            var response = await _service.LoginUserAsync(loginDto);
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeFalse();
            response.Data.Should().BeNull();
            response.Message.Should().Be("Username does not exist.");
        }

        [Fact]
        public async Task LoginUserAsync_ShouldReturnFailure_WhenPasswordIsIncorrect()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                PasswordHash = new PasswordHasher<User>().HashPassword(null!, "correct_password"),
                Role = new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "PLAYER"
                }
            };
            _userRepoMock.Setup(r => r.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                         .ReturnsAsync(user);
            var loginDto = new LoginDto(
                Username: "testuser",
                Password: "wrong_password"
            );
            var response = await _service.LoginUserAsync(loginDto);
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeFalse();
            response.Data.Should().BeNull();
            response.Message.Should().Be("Password is incorrect.");
        }
    }
}
