
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using UserManagement.Models;

public class AuditLogs
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public long UserId { get; set; }

    public virtual User User { get; set; } = default!;

    public DateTime Date { get; set; }

    public string Action { get; set; } = default!;
    public string EntityName { get; set; } = default!;

    public AuditLogs()
    {
        Date = DateTime.Now;
    }
}
