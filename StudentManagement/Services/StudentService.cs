using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;
using StudentManagement.services;

namespace StudentManagement.Services;

public class StudentService(ApplicationDbContext context) : IStudentService
{
    public async Task AddStudentAsync(Student student)
    {
        if(await context.Students.AnyAsync(s=>s.MSHS == student.MSHS))
            throw new ApplicationException("Mã sinh viên đã tồn tại");
        if (!await context.ClassInfos.AnyAsync(c => c.Id == student.ClassId))
            throw new ApplicationException("Lớp học không tồn tại");
        try
        {
            await context.Students.AddAsync(student);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new ApplicationException("Có lỗi khi thêm sinh viên: ",e);
        }
    }

    public async Task<List<Student>> GetAllStudentsAsync()
    {
        return await context.Students.Include(s=>s.ClassInfo).ToListAsync();
    }

    public async Task<List<Student>> GetStudentsByClassIdAsync(int classId)
    {
        if (!await context.ClassInfos.AnyAsync(x => x.Id == classId))
            throw new ApplicationException($"Không tìm thấy lớp học với id: {classId}");
        return await context.Students
            .Include(s=>s.ClassInfo)
            .Where(s=>s.ClassId==classId)
            .ToListAsync();
    }
    public async Task<Student> GetStudentByStudentIdAsync(int studentId)
    {
        return await context.Students
            .Include(s=>s.ClassInfo)
            .Include(s=>s.Grades)
            .FirstOrDefaultAsync(s=>s.Id == studentId) ?? throw new ApplicationException("Học sinh không tồn tại");
    }

    public async Task UpdateStudentAsync(Student student)
    {
        if(!await context.ClassInfos.AnyAsync(c => c.Id == student.ClassId))
            throw new ApplicationException("Lớp học không tồn tại");
        var existingStudent = await context.Students.FirstOrDefaultAsync(s=>s.Id==student.Id)
            ??throw new ApplicationException("Học sinh không tồn tại");
        existingStudent.Address = student.Address;
        existingStudent.Birthday = student.Birthday;
        existingStudent.FirstName = student.FirstName;
        existingStudent.MiddleName = student.LastName;
        context.Students.Update(existingStudent);
        await context.SaveChangesAsync();
    }
    public async Task DeleteStudentAsync(int studentId)
    {
        var student= await context.Students.Include(s=>s.Grades)
                         .FirstOrDefaultAsync(s=>s.Id == studentId)
                     ??throw new ApplicationException($"Không tìm thấy học sinh với id {studentId}");
        if (student.Grades.Any())
        {
            context.Grades.RemoveRange(student.Grades);
        }

        try
        {
            context.Students.Remove(student);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine(e);
            throw new ApplicationException($"Lỗi khi xóa sinh viên: {e.InnerException?.Message}",e);
        }
      
    }
    public async Task<ICollection<Grade>> GetGradesBySemesterAsync(int studentId,int semester)
    {
        var student = await context.Students
                          .Include(s => s.Grades)
                          .FirstOrDefaultAsync(s => s.Id == studentId)
                      ?? throw new ApplicationException("Không tìm thấy sinh viên");
        
        var grades= student.Grades.
            Where(s => s.Semester == semester).ToList();
        return grades.Any() ? grades : throw new ApplicationException($"Không tìm thấy điểm số cho học kì {semester}");
    }
    public async Task<float> GetAverageGradeBySemesterAsync(int studentId, int semesterId)
    {
            if (!context.Semesters.Any(s => s.Id == semesterId))
                throw new ApplicationException($"Không tìm thấy học kì với Id: {semesterId}");
            var student = await GetStudentByStudentIdAsync(studentId);
            return student.CalculateAverageScoreBySemester(semesterId);
    }

}