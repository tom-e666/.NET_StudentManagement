using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;

namespace StudentManagement.Services;

public class ClassInfoService(ApplicationDbContext context)
{
        public async Task AddClassInfoAsync(ClassInfo classInfo)
        {
                if (await context.ClassInfos.AnyAsync(c => c.ClassCode == classInfo.ClassCode))
                        throw new ApplicationException($"Mã lớp {classInfo.ClassCode} đã tồn tại");
                try
                {
                        await context.ClassInfos.AddAsync(classInfo);
                        await context.SaveChangesAsync();
                }
                catch (DbException e)
                {
                        throw new ApplicationException(e.Message,e.InnerException);
                }
        }

        public async Task<List<ClassInfo>> GetClassInfosAsync()
        {
                return await context.ClassInfos.ToListAsync();
        }

        public async Task UpdateClassInfoAsync(ClassInfo classInfo)
        {
                var existing = await context.ClassInfos
                                       .FirstOrDefaultAsync(c => c.ClassCode == classInfo.ClassCode)
                               ?? throw new ApplicationException($"Không tìm thấy lớp với id: {classInfo.Id}");
                existing.ClassCode = classInfo.ClassCode;
                existing.ClassName = classInfo.ClassName;
                try
                {
                        context.ClassInfos.Update(existing);
                        await context.SaveChangesAsync();
                }
                catch (DbUpdateException e)
                {
                        Console.WriteLine(e.InnerException);
                        throw new ApplicationException(e.Message);
                }
        }

       public async Task DeleteClassInfoAsync(ClassInfo classInfo)
        {
                var existing = context.ClassInfos
                                       .Include(c => c.Students)
                                       .FirstOrDefault(c => c.Id == classInfo.Id)
                               ?? throw new ApplicationException($"Lớp học không tồn tại");
                if (existing.Students.Any())
                        throw new ApplicationException($"Có sinh viên thuộc lớp ${classInfo.ClassName} không thể xóa");
                context.ClassInfos.Remove(existing);
                await context.SaveChangesAsync();
        }
    
}