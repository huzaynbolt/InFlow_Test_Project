namespace UserManagement.Web.Models;

public class PaginationViewModel<T>
{
    public List<T> Items { get; set; } = default!;

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}
