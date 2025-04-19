using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;

namespace StudentManagement.services;

public class StudentService:IStudentService
{
    protected ApplicationDbContext context;

    public StudentService(ApplicationDbContext _context)
    {
        context = _context;
    }
    public async Task AddStudentAsync(Student student)
    {
        var res= await context.Students.FindAsync(student.Id);
        if(res!=null)
            throw new ApplicationException("Mã sinh viên đã tồn tại");
        if (!context.ClassInfos.Any(c => c.Id == student.ClassId))
            throw new ApplicationException("Lớp học không tồn tại");
        try
        {
            context.Students.Add(student);
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
        return await context.Students.Include(s=>s.ClassInfo).Where(s=>s.ClassId==classId).ToListAsync();
    }

    public async Task<Student> GetStudentByStudentIdAsync(int studentId)
    {
        return await context.Students
            .Include(s=>s.ClassInfo)
            .Include(s=>s.Grades)
            .FirstOrDefaultAsync(s=>s.Id == studentId)?? throw new ApplicationException("Học sinh không tồn tại");
    }

    public async Task UpdateStudentAsync(Student student)
    {
        if (!context.ClassInfos.Any(c => c.Id == student.ClassId))
            throw new ApplicationException("Lớp học không tồn tại");
        if (!context.Students.Any(s => s.Id == student.Id))
        {
            throw new ApplicationException("Học sinh không tồn tại");
        }
        context.Students.Update(student);
        await context.SaveChangesAsync();
    }

    public async Task DeleteStudentAsync(Student student)
    {
        if (!context.Students.Any(s => s.Id == student.Id))
            throw new ApplicationException("Học sinh không tồn tại");
        try
        {
            context.Students.Remove(student);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        
    }
    public async Task<ICollection<Grade>> GetGradesAsync(string studentId,int semester)
    {
        return context.Students.FindAsync(studentId).Result.Grades ?? throw new ApplicationException("Không tìm thấy học sinh");
    }

    public async Task<float> GetGradeAverageAsync(int studentId, int semester)
    {
        var student = await GetStudentByStudentIdAsync(studentId);
        return student.CalculateAverageScore();
    }

}