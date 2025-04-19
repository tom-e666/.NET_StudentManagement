using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models;

public class Subject
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(50)]
    [RegularExpression(@"^[A-z0-9]+$",ErrorMessage ="Mã lớp chỉ bao gồm chữ in hoa và số")]
    public required String SubjectCode { get; set; }
    [Required]
    [StringLength(50)]
    public required String Name { get; set; }
    public virtual string GetSubjectInfo() => $"{SubjectCode} - {Name}";
    public virtual List<Grade> Grades { get; set; }=new List<Grade>();
    
}