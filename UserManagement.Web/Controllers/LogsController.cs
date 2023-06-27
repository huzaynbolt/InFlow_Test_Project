using System;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Services.Interfaces;
using UserManagement.Web.Models;

namespace UserManagement.Web.Controllers;
public class LogsController : Controller
{
    private readonly IAuditLogService _auditLogService;
    public LogsController(IAuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }

    public async Task<ActionResult> Index(int page = 1)
    {
        var items = (await _auditLogService.GetAllAsync()).Select( c => AuditLogsViewModel.FromLogModel(c));
   
        var totalLogs = items.Count();
        var pageSize = 50;
        var totalPages = (int)Math.Ceiling((double)totalLogs / pageSize);

        // Adjust the current page if it goes beyond the valid range
        page = Math.Max(1, Math.Min(page, totalPages));

        // Get the logs for the current page
        List<AuditLogsViewModel> pagedLogs = items.Skip((page - 1) * pageSize).Take(50).ToList();

        var paginatedItems = new PaginationViewModel<AuditLogsViewModel>()
        {
            CurrentPage = page,
            Items = pagedLogs,
            TotalPages = totalPages,
        };
        ViewData["Title"] = "View Audit logs";
        return View(paginatedItems);
    }

    [HttpPost]
    public async Task<ActionResult> LogDetails(long id)
    {
        var item = await _auditLogService.GetWithUserDetailsAsync(id);
        TempData["LogDetails"] = $"{item.Action} action was performed under {item.User.Forename} {item.User.Surname} on {item.Date.ToLongDateString()}";
        return RedirectToAction("Index");
    }


}
