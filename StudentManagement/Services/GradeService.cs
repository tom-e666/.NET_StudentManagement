using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;
using StudentManagement.services;

namespace StudentManagement.Services;

public class GradeService(ApplicationDbContext Context):IGradeService
{
    public async Task AddGradeAsync(Grade grade)
    {
        if (!await Context.Students.AnyAsync(e => e.Id == grade.StudentId))
            throw new ApplicationException($"Không có sinh viên với id {grade.StudentId}");
        if(!await Context.Subjects.AnyAsync(e => e.Id == grade.SubjectId))
            throw new ApplicationException($"Không có lớp với mã lớp : {grade.SubjectId}");
        await Context.Grades.AddAsync(grade);
        await Context.SaveChangesAsync();
    }

    public async Task<List<Grade>> GetGradesAsync()
    {
        return await Context.Grades.ToListAsync();
    }
    public async Task UpdateGradeAsync(Grade grade)
    {
        if (!await Context.Grades.AnyAsync(e => e.Id == grade.Id))
            throw new ApplicationException($"Không tồn tại bản ghi điểm");
        if (!await Context.Subjects.AnyAsync(e => e.Id == grade.SubjectId))
            throw new ApplicationException($"Không tồn tại môn học với mã:{grade.SubjectId}");
        if(!await Context.Students.AnyAsync(e => e.Id == grade.StudentId))
            throw new ApplicationException($"Không tồn tại sinh viên với id: {grade.StudentId}");
        var existing = await Context.Grades.FirstOrDefaultAsync(e => e.Id == grade.Id) ??
                       throw new ApplicationException("Bản ghi không tồn tại");
        try
        {
            existing.Score = grade.Score;
            existing.Semester = grade.Semester;
            Context.Grades.Update(existing);
            await Context.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine(e);
            throw new ApplicationException(e.Message,e.InnerException);
        } 
    }
}