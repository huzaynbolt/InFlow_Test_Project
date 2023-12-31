using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;

namespace UserManagement.Data.Tests;

public class UserServiceTests
{
    [Fact]
    public void GetAll_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.GetAll();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeSameAs(users);
    }

    [Fact]
    public void Create_WhenEntityIsCreated_MustCallContext()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        _dataContext.Setup(c => c.Create<User>(It.IsAny<User>()));
        var service = CreateService();

        // Act: Invokes the method under test with the arranged parameters.
        service.Create(It.IsAny<User>());

        // Assert: Verifies that the action of the method under test behaves as expected.
        _dataContext.Verify(c=>c.Create<User>(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public void Update_WhenEntityIsUpdated_MustCallContext()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        _dataContext.Setup(c => c.Create<User>(It.IsAny<User>()));
        var service = CreateService();

        // Act: Invokes the method under test with the arranged parameters.
        service.Update(It.IsAny<User>());

        // Assert: Verifies that the action of the method under test behaves as expected.
        _dataContext.Verify(c => c.Update<User>(It.IsAny<User>()), Times.Once);
    }

    private IQueryable<User> SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", bool isActive = true)
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
        }.AsQueryable();

        _dataContext
            .Setup(s => s.GetAll<User>())
            .Returns(users);

        return users;
    }

    private readonly Mock<IDataContext> _dataContext = new();
    private UserService CreateService() => new(_dataContext.Object);
}
