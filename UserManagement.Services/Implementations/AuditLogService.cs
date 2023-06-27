

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Services.Interfaces;

namespace UserManagement.Services.Implementations;
public class AuditLogService : IAuditLogService
{
    private readonly IDataContext _dataAccess;
    public AuditLogService(IDataContext dataAccess) => _dataAccess = dataAccess;
    public async Task CreateAsync(AuditLogs log) => await Task.Run(() => { _dataAccess.Create(log); });
    public Task DeleteAsync(long id) => throw new NotImplementedException();

    public async Task<IEnumerable<AuditLogs>> GetAllAsync()
    {
       
        return await _dataAccess.GetAll<AuditLogs>().Include(c => c.User).ToListAsync();
    }
    public async Task<AuditLogs> GetAsync(long id) => await Task.Run<AuditLogs>( () => _dataAccess.Get<AuditLogs>(c=>c.Id == id)!);

    public async Task<AuditLogs> GetWithUserDetailsAsync(long id) => await Task.Run<AuditLogs>(() => _dataAccess.Get<AuditLogs>(c => c.Id == id, "User")!);
    public Task UpdateAsync(AuditLogs logToUpdate) => throw new NotImplementedException();
}
