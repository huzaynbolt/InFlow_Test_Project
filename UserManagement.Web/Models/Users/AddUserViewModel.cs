using System;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Web.Models.Users;

public class AddUserViewModel
{
    public AddUserViewModel()
    {
        
    }
    public AddUserViewModel(string forename, string surname, string email, DateTime dob)
    {
        Forename = forename;
        Surname = surname;
        Email = email;
        DateOfBirth = dob;
    }
    [Required]
    public string? Forename { get; set; }

    [Required]
    public string? Surname { get; set; }

    [EmailAddress]
    public string? Email { get; set; }
    public bool IsActive { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }
}
