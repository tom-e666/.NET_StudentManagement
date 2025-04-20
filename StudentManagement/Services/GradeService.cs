using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;
using StudentManagement.services;

namespace StudentManagement.Services;

public class GradeService(ApplicationDbContext context):IGradeService
{
    public async Task AddGradeAsync(Grade grade)
    {
        if (!await context.Students.AnyAsync(e => e.Id == grade.StudentId))
            throw new ApplicationException($"Không có sinh viên với id {grade.StudentId}");
        if(!await context.Subjects.AnyAsync(e => e.Id == grade.SubjectId))
            throw new ApplicationException($"Không có lớp với mã lớp : {grade.SubjectId}");
        await context.Grades.AddAsync(grade);
        await context.SaveChangesAsync();
    }

    public async Task<List<Grade>> GetGradesAsync()
    {
        return await context.Grades.ToListAsync();
    }
    public async Task UpdateGradeAsync(Grade grade)
    {
        if (!await context.Grades.AnyAsync(e => e.Id == grade.Id))
            throw new ApplicationException($"Không tồn tại bản ghi điểm");
        if (!await context.Subjects.AnyAsync(e => e.Id == grade.SubjectId))
            throw new ApplicationException($"Không tồn tại môn học với mã:{grade.SubjectId}");
        if(!await context.Students.AnyAsync(e => e.Id == grade.StudentId))
            throw new ApplicationException($"Không tồn tại sinh viên với id: {grade.StudentId}");
        var existing = await context.Grades.FirstOrDefaultAsync(e => e.Id == grade.Id) ??
                       throw new ApplicationException("Bản ghi không tồn tại");
        try
        {
            existing.Score = grade.Score;
            existing.Semester = grade.Semester;
            context.Grades.Update(existing);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine(e);
            throw new ApplicationException(e.Message,e.InnerException);
        } 
    }
}