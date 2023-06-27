using System.Linq;
using System.Threading.Tasks;
using UserManagement.Data;
using UserManagement.Services.Implementations;

namespace UserManagement.Services.Tests;
public class AuditLogServiceTests
{


    [Fact]
    public async Task Create_WhenEntityIsCreated_MustCallContext()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        _dataContext.Setup(c => c.Create(It.IsAny<AuditLogs>()));
        var service = CreateService();

        // Act: Invokes the method under test with the arranged parameters.
        await service.CreateAsync(It.IsAny<AuditLogs>());

        // Assert: Verifies that the action of the method under test behaves as expected.
        _dataContext.Verify(c => c.Create(It.IsAny<AuditLogs>()), Times.Once);
    }

    private IQueryable<AuditLogs> SetupLogs()
    {
        var logs = new[]
        {
            new AuditLogs
            {
               Date = System.DateTime.UtcNow,
               Id = 1,
               EntityName = "Test",
               Action = "Create",
               UserId = 1,
            }
        }.AsQueryable();

        _dataContext
            .Setup(s => s.GetAll<AuditLogs>())
            .Returns(logs);

        return logs;
    }

    private readonly Mock<IDataContext> _dataContext = new();
    private AuditLogService CreateService() => new(_dataContext.Object);
}
