using StudentManagement.Models;
using StudentManagement.services;

namespace StudentManagement.Services;

public class GradeService(ApplicationDbContext Context):IGradeService
{
    public async Task AddGradeAsync(Grade grade)
    {
        
    }

    public async Task<List<Grade>> GetGradesAsync()
    {
        
    }
    public UpdateGradeAsync(Grade grade);
}