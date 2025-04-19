using StudentManagement.Models;

namespace StudentManagement.services;

public interface IStudentService
{ 
    Task AddStudentAsync(Student student);
    Task<List<Student>> GetAllStudentsAsync();
    Task<List<Student>> GetStudentsByClassIdAsync(int classId);
    Task<Student> GetStudentByStudentIdAsync(int studentId);
    Task UpdateStudentAsync(Student student);
    Task DeleteStudentAsync(int studentId);
    Task <ICollection<Grade>> GetGradesBySemesterAsync(int studentId,int semester);
    Task<float> GetAverageGradeBySemesterAsync(int studentId, int semester);
    
}

public interface IClassInfoService
{
    Task AddClassInfoAsync(ClassInfo classInfo);
    Task<List<ClassInfo>> GetClassInfosAsync();
    Task UpdateClassInfoAsync(ClassInfo classInfo);
    Task DeleteClassInfoAsync(ClassInfo classInfo);
}

public interface IGradeService
{
    Task AddGradeAsync(Grade grade);
    Task<List<Grade>> GetGradesAsync();
    Task UpdateGradeAsync(Grade grade);
    
}

public interface ISubjectService

{
    Task AddSubjectAsync(Subject subject);
    Task<List<Subject>> GetSubjectsAsync();
    Task UpdateSubjectAsync(Subject subject);
    Task DeleteSubjectAsync(Subject subject);
}
public interface IUser
{
    Task<User> GetUserByUsernameAsync(string username);
    Task AddUserAsync(User user);
    Task<User> GetUserByEmailAsync(string email);
    Task UpdateUserAsync(User user);
    Task<bool> AuthenticateUserAsync(string username, string password);
}

