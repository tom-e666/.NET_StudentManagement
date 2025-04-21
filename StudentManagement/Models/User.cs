using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace StudentManagement.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(50)]
    public required string Username { get; set; }
    
    [Required]
    [StringLength(256)]
    public required string PasswordHash { get; set; }
    
    [StringLength(256)]
    public required string Email { get; set; }
    public required RoleEnum Role { get; set; }
    public virtual string GetRole()=>Role.ToString()?? "Unknown";
}
[Flags]
public enum RoleEnum
{
    NONE=0,
    TEACHER = 1<<0,
    STUDENT = 1<<1,
    INSPECTOR = 1<<2,
    ADMIN = 1<<3,
}