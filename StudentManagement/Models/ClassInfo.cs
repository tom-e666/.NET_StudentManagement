using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace StudentManagement.Models;

public class ClassInfo
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(50)]
    [RegularExpression(@"^[A-Z0-9]+$", ErrorMessage = "Mã lớp chỉ bao gồm chữ cái in hoa và số")]
    public required string ClassCode { get; set; }
    [Required]
    [StringLength(100)]
    public required string ClassName { get; set; }
    [Required]
    public required DateTime StartDate{ get; set; }
    public string GetClassInfo()=> $"{ClassCode} - {ClassName}";
    public ICollection<Student> Students { get; set; } = new List<Student>();
}