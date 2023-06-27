using System;

namespace UserManagement.Web.Models;

public class AuditLogsViewModel
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public string UserFullName { get; set; } = default!;

    public DateTime Date { get; set; }

    public string Action { get; set; } = default!;
    public string EntityName { get; set; } = default!;


    public static AuditLogsViewModel FromLogModel(AuditLogs log)
    {
        return new AuditLogsViewModel {
            Action = log.Action,
            Date = log.Date,
            EntityName = log.EntityName,
            UserFullName = $"{log.User.Forename} {log.User.Surname}",
            UserId = log.UserId,
            Id = log.Id
        };
    }
}

public class AuditLogsViewModelList
{
    public AuditLogsViewModelList(IEnumerable<AuditLogsViewModel> items)
    {
        Items = items;
    }
    public IEnumerable<AuditLogsViewModel> Items { get; set; }
}
