using System;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Services.Interfaces;
using UserManagement.Web.Models;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;

    private readonly IAuditLogService _auditLogService;
    public UsersController(IUserService userService, IAuditLogService auditLogService)
    {
        _userService = userService;

        _auditLogService = auditLogService;
    }

    [HttpGet]
    public ViewResult List()
    {
        var items = _userService.GetAll().Select(ProjectUserViewModel());
        var model = MapUserViewModel(items);
        ViewData["Title"] = "List all Users";
        return View(model);
    }

    [HttpGet]
    [Route("active-state")]
    public ViewResult ActiveState(bool isActive)
    {
        var items = _userService.FilterByActive(isActive).Select(ProjectUserViewModel());
        var model = MapUserViewModel(items);
        ViewData["Title"] = $"Get {(isActive ? "active" : "inactive")} users";
        return View("/Views/Users/List.cshtml", model);
    }


    [HttpGet]
    [Route("create")]
    public ViewResult Create()
    {
        ViewData["Title"] = $"Add new user";
        return View();
    }

    [HttpPost]
    [Route("create")]
    public async Task<ActionResult> Create(AddUserViewModel user)
    {
        if (ModelState.IsValid)
        {
            var newUser = ProjectUserForCreate(user);
            _userService.Create(newUser);
            await _auditLogService.CreateAsync(new AuditLogs
            {
                Action = "Create",
                EntityName = "User",
                UserId = newUser.Id,
            });
            return RedirectToAction("List");
        }

        return View(user);
    }

    [HttpGet]
    [Route("edit")]
    public async Task<ViewResult> Edit(int id)
    {
        ViewData["Title"] = $"Edit user";
        var user = await _userService.Get(id);
        var mappedUser = new EditUserViewModel
        {
            DateOfBirth = user!.DateOfBirth,
            Email = user!.Email,
            Forename = user!.Forename,
            IsActive = user!.IsActive,
            Id = user.Id,
            Surname = user!.Surname
        };
        return View(mappedUser);
    }

    [HttpPost]
    [Route("edit")]
    public async Task<ActionResult> Edit(EditUserViewModel user)
    {
        var updatedUser = (await _userService.Get(user.Id))!;
        updatedUser.Surname = user.Surname!;
        updatedUser.DateOfBirth = user!.DateOfBirth;
        updatedUser.Email = user.Email!;
        updatedUser.Forename = user.Forename!;
        updatedUser.Id = user.Id;
        updatedUser.IsActive = user.IsActive;

        _userService.Update(updatedUser);
        await _auditLogService.CreateAsync(new AuditLogs
        {
            Action = "Update",
            EntityName = "User",
            UserId = user.Id,
        });
        return RedirectToAction("List");
    }

    [HttpGet]
    [Route("details")]
    public async Task<ViewResult> Details(int id)
    {
        var user = await _userService.GetUserWithLogs(id);
        ViewData["Title"] = $"View user";
        var mappedUser = new UserListItemViewModel
        {
            DateOfBirth = user!.DateOfBirth,
            Email = user!.Email,
            Forename = user!.Forename,
            IsActive = user!.IsActive,
            Id = user.Id,
            Surname = user!.Surname,
            Logs = user.AuditLogs != null ?
                       user.AuditLogs.Select(c => AuditLogsViewModel.FromLogModel(c)).ToList() : new List<AuditLogsViewModel>()
        };
        await _auditLogService.CreateAsync(new AuditLogs
        {
            Action = "Read",
            EntityName = "User",
            UserId = user.Id
        });
        return View(mappedUser);
    }


    [HttpGet]
    [Route("delete")]
    public async Task<ViewResult> Delete(int id)
    {
        var user = await _userService.Get(id);
        ViewData["Title"] = $"Delete user";
        ViewData["IsDeleteEnable"] = true;
        var mappedUser = new UserListItemViewModel
        {
            DateOfBirth = user!.DateOfBirth,
            Email = user!.Email,
            Forename = user!.Forename,
            IsActive = user!.IsActive,
            Id = user.Id,
            Surname = user!.Surname
        };
        return View(mappedUser);
    }

    [HttpPost]
    [Route("delete")]
    public async Task<ActionResult> Delete(long id)
    {
        await _userService.Delete(id);

        await _auditLogService.CreateAsync(new AuditLogs
        {
            Action = "Delete",
            EntityName = "User",
            UserId = id,
        });
        return RedirectToAction("List");
    }


    private static Func<User, UserListItemViewModel> ProjectUserViewModel() => p => new UserListItemViewModel
    {
        Id = p.Id,
        Forename = p.Forename,
        Surname = p.Surname,
        Email = p.Email,
        IsActive = p.IsActive,
        DateOfBirth = p.DateOfBirth,
        Logs = p.AuditLogs != null ? p.AuditLogs.Select(c=> AuditLogsViewModel.FromLogModel(c)).ToList()
                                    : new List<AuditLogsViewModel>()
    };

    private static UserListViewModel MapUserViewModel(IEnumerable<UserListItemViewModel> items) => new ()
    {
        Items = items.ToList()
    };

    private static User ProjectUserForCreate(AddUserViewModel user)
    {
        return new User
        {
            Forename = user.Forename!,
            Surname = user.Surname!,
            Email = user.Email!,
            IsActive = user.IsActive,
            DateOfBirth = user.DateOfBirth
        };
    } 
    


}
