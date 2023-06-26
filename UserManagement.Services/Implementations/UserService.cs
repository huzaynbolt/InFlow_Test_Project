using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;
    public UserService(IDataContext dataAccess) => _dataAccess = dataAccess;

    public void Create(User user) => _dataAccess.Create(user);
    public async Task Delete(long userId)
    {
        var user = await _dataAccess.Get<User>(c => c.Id == userId);
        _dataAccess.Delete(user!);
    }

    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    public IEnumerable<User> FilterByActive(bool isActive)
    {
        return _dataAccess.GetAll<User>(c => c.IsActive == isActive).ToList();
    }

    public async Task<User?> Get(long userId) => await _dataAccess.Get<User>(c =>c.Id == userId);

    public IEnumerable<User> GetAll() => _dataAccess.GetAll<User>();

    public void Update(User userToUpdate)
    {
        _dataAccess.Update(userToUpdate);
        
    }
}
