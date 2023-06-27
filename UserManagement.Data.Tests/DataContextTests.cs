using System.Linq;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;

namespace UserManagement.Data.Tests;

public class DataContextTests
{

 

    [Fact]
    public void GetAll_WhenNewEntityAdded_MustIncludeNewEntity()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();
        context.Users!.RemoveRange(context.Users);

        var entity = new User
        {
            Forename = "Brand New",
            Surname = "User",
            Email = "brandnewuser@example.com",
        };
        context.Create(entity);

        // Act: Invokes the method under test with the arranged parameters.
        var result = context.GetAll<User>();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result
            .Should().Contain(s => s.Email == entity.Email)
            .Which.Should().BeEquivalentTo(entity);
    }

    [Fact]
    public void GetAll_WhenDeleted_MustNotIncludeDeletedEntity()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();
        var entity = context.GetAll<User>().First();
        context.Delete(entity);

        // Act: Invokes the method under test with the arranged parameters.
        var result = context.GetAll<User>();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().NotContain(s => s.Email == entity.Email);
    }


    [Fact]
    public void Create_WhenAdded_MustBeIncludeInListOfEntity()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();
        var entity = new User { DateOfBirth = new System.DateTime(1999, 02, 12), Email = "test@test.com",
            Forename = "test-fore", Surname = "test-sur", IsActive = true } ;


        // Act: Invokes the method under test with the arranged parameters.
        context.Create(entity);

        // Assert: Verifies that the action of the method under test behaves as expected.
        var testUser = context.GetAll<User>(c=>c.Email == "test@test.com");
        testUser.Should().NotBeNull();
    }


    [Fact]
    public void Update_WhenEntityIsModified_MustBeReflectedInListOfEntity()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();
        var entity = new User
        {
            Forename = "Brand New",
            Surname = "User",
            Email = "brandnewuser@example.com"
        };
        context.Create(entity);

        entity.Forename = "Updated Brand New";

        // Act: Invokes the method under test with the arranged parameters.
        context.Update(entity);

        // Assert: Verifies that the action of the method under test behaves as expected.
        var testUser = context.GetAll<User>(c => c.Forename == "Updated Brand New");
        testUser.Should().NotBeNull();
    }

    private DataContext CreateContext() => new();
}
