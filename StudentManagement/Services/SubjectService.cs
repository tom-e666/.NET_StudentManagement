using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;
using StudentManagement.services;

namespace StudentManagement.Services;

public class SubjectService(ApplicationDbContext context): ISubjectService
{
    public async Task AddSubjectAsync(Subject subject)
    {
        if(await context.Subjects.AnyAsync(s => s.SubjectCode == subject.SubjectCode))
            throw new ArgumentException("SubjectCode đã sử dụng");
        try
        {
            await context.Subjects.AddAsync(subject);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new ApplicationException(e.Message);
        }
        
    }
    public async Task<List<Subject>> GetSubjectsAsync()
    {
        return await context.Subjects.ToListAsync() ??
               throw new ApplicationException("Không có môn học nào để trình bày");
    }

    public async Task UpdateSubjectAsync(Subject subject)
    {
        var existing = context.Subjects.Find(subject.Id)
                       ?? throw new ApplicationException("Không tìm thấy môn học");
        if (existing.SubjectCode!=subject.SubjectCode
            && await context.Subjects.AnyAsync(s => s.SubjectCode == subject.SubjectCode))
            throw new ArgumentException("SubjectCode đã được sử dụng");
        existing.Name = subject.Name;
        existing.SubjectCode = subject.SubjectCode;
        try
        {
            context.Subjects.Update(existing);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine(e);
            throw new ApplicationException(e.Message,e.InnerException);
        }
        
    }

    public async Task DeleteSubjectAsync(int id)
    {
        var exisisting = await context.Subjects.Include(e=>e.Grades)
                             .FirstOrDefaultAsync(e=>e.Id==id)??
                         throw new ApplicationException("Không tìm thấy môn học");
        try
        {
            if (exisisting.Grades.Any())
            {
                context.Grades.RemoveRange(exisisting.Grades);
                Console.WriteLine(" Tồn tại bản ghi điểm, hiện tại đã bị xóa");
            }
                
            context.Subjects.Remove(exisisting);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new ApplicationException(e.Message);
        }
    }
}