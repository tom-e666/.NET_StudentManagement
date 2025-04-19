using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models;

public class Semester
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(50)]
    [RegularExpression(@"^[A-Z0-9]+$", ErrorMessage = "Chỉ dùng chữ IN HOA và số")]
    public required string CodeName { get; set; }
}