using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Services.Interfaces;
using UserManagement.Web.Models.Users;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Data.Tests;

public class UserControllerTests
{
    [Fact]
    public void List_WhenServiceReturnsUsers_ModelMustContainUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = controller.List();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public void Create_WhenUserIsCreated_ServiceMustBeCalledToCreateUser()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        _userService.Setup(c => c.Create(It.IsAny<User>()));
        var controller = CreateController();

        // Act: Invokes the method under test with the arranged parameters.
        var user = new AddUserViewModel
        {
            DateOfBirth = new System.DateTime(1992, 11, 01),
            Email = "test-email@test.com",
            Forename = "fore-test",
            Surname = "surname"
        };
        var result = controller.Create(user);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _userService.Verify(c => c.Create(It.Is<User>(c => user.Email == c.Email &&
                                                           user.DateOfBirth == c.DateOfBirth &&
                                                           user.IsActive == c.IsActive &&
                                                           user.Forename == c.Forename &&
                                                           user.Surname == c.Surname)), Times.Once);
    }

    [Fact]
    public void Edit_WhenUserIsEdited_ServiceMustBeCalledToUpdateUser()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        _userService.Setup(c => c.Create(It.IsAny<User>()));
        _userService.Setup(c => c.Get(It.IsAny<long>())).ReturnsAsync(new User
        {
            DateOfBirth = new System.DateTime(1992, 11, 01),
            Email = "email1@test.com",
            Forename = "fore-test-1",
            Surname = "surname-1",
            Id = 12
        });
        var controller = CreateController();

        // Act: Invokes the method under test with the arranged parameters.
        var user = new EditUserViewModel
        {
            DateOfBirth = new System.DateTime(1992, 11, 01),
            Email = "test-email-2@test.com",
            Forename = "fore-test-2",
            Surname = "surname-1",
            Id = 12
        };
        var result = controller.Edit(user);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _userService.Verify(c => c.Update(It.Is<User>(c => user.Email == c.Email &&
                                                           user.DateOfBirth == c.DateOfBirth &&
                                                           user.IsActive == c.IsActive &&
                                                           user.Forename == c.Forename &&
                                                           user.Id == c.Id &&
                                                           user.Surname == c.Surname)), Times.Once);
    }

    [Fact]
    public void Details_WhenUserIsViewed_ServiceMustBeCalledToGetUser()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var userId = 12;
        _userService.Setup(c => c.Get(It.IsAny<long>())).ReturnsAsync(new User
        {
            DateOfBirth = new System.DateTime(1992, 11, 01),
            Email = "email1@test.com",
            Forename = "fore-test-1",
            Surname = "surname-1",
            Id = userId
        });
        var controller = CreateController();

        // Act: Invokes the method under test with the arranged parameters.
        var result = controller.Details(userId);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _userService.Verify(c => c.Get(It.Is<long>(c => c == userId)), Times.Once);
    }




    private User[] SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", bool isActive = true)
    {
        var users = new[]
        {
            new User
            {
                Forename = forename,
                Surname = surname,
                Email = email,
                IsActive = isActive
            }
        };

        _userService
            .Setup(s => s.GetAll())
            .Returns(users);

        return users;
    }

    private readonly Mock<IUserService> _userService = new();

    private readonly Mock<IAuditLogService> _auditLogService = new();
    private UsersController CreateController() => new(_userService.Object, _auditLogService.Object);
}
