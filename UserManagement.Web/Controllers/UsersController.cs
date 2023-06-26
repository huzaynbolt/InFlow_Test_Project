using System;
using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

    [HttpGet]
    public ViewResult List()
    {
        var items = _userService.GetAll().Select(ProjectUserViewModel());
        var model = MapUserViewModel(items);

        return View(model);
    }

    [HttpGet]
    [Route("active-state")]
    public ViewResult ActiveState(bool isActive)
    {
        var items = _userService.FilterByActive(isActive).Select(ProjectUserViewModel());
        var model = MapUserViewModel(items);

        return View("/Views/Users/List.cshtml", model);
    }

    private static Func<User, UserListItemViewModel> ProjectUserViewModel() => p => new UserListItemViewModel
    {
        Id = p.Id,
        Forename = p.Forename,
        Surname = p.Surname,
        Email = p.Email,
        IsActive = p.IsActive,
        DateOfBirth = p.DateOfBirth
    };

    private static UserListViewModel MapUserViewModel(IEnumerable<UserListItemViewModel> items) => new ()
    {
        Items = items.ToList()
    };
}
