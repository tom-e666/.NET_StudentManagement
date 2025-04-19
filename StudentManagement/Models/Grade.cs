using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models;

public class Grade
{
    [Key]
    public int Id { get; set; }
    [Required]
    [Range(0.0f,10.0f, ErrorMessage = "Nhập điểm x.x trong khoảng từ 0 đến 10")]
    public required float Score { get; set; }
    [Required]
    [ForeignKey("Student")]
    public required int StudentId { get; set; }
    [Required]
    [ForeignKey("Subject")]
    public required int SubjectId { get; set; }
    [Required]
    public required string Semester { get; set; }
    public virtual Student Student { get; set; }
    public virtual Subject Subject { get; set; }
}