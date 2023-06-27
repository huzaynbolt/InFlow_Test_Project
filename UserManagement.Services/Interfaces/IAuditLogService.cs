

using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserManagement.Services.Interfaces;
public interface IAuditLogService
{

    Task<IEnumerable<AuditLogs>> GetAllAsync();

    Task CreateAsync(AuditLogs log);
    Task UpdateAsync(AuditLogs logToUpdate);
    Task DeleteAsync(long id);

    Task<AuditLogs> GetAsync(long id);

    Task<AuditLogs> GetWithUserDetailsAsync(long id);
}
