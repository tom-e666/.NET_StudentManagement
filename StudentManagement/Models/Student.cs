using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models;

public class Student
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(50)]
    [RegularExpression(@"^[0-9]+$",ErrorMessage = "Mã học sinh chỉ bao gồm số")]
    public required string MSHS { get; set; }
    [Required]
    [StringLength(50)]
    public required string FirstName { get; set; }//Tên
    [Required]
    [StringLength(50)]
    public required string LastName { get; set; }//Họ
    [Required]
    [StringLength(50)]
    public required string MiddleName { get; set; }//Lót
    [Required]
    public required DateTime Birthday { get; set; }
    [Required]
    [StringLength(100)]
    public required string Address { get; set; }
    
    [Required]
    [ForeignKey("ClassInfo")]
    public required int ClassId { get; set; }
    
    public virtual string GetFullName() => $"{LastName} {MiddleName}  {FirstName}";
    public virtual ClassInfo ClassInfo { get; set; }
    public virtual ICollection<Grade> Grades { get; set; }= new List<Grade>();
    public virtual float CalculateAverageScore()
    {
        if(Grades == null || Grades.Count == 0) return 0;
        return Grades.Average(x => x.Score);
    }
}