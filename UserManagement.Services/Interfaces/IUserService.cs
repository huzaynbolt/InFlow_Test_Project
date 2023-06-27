using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface IUserService 
{
    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    IEnumerable<User> FilterByActive(bool isActive);
    IEnumerable<User> GetAll();

    void Create(User user);
    void Update(User userToUpdate);
    Task Delete(long userId);

    Task<User?> Get(long userId);
    Task<User?> GetUserWithLogs(long userId);
}
