using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;
using StudentManagement.services;

namespace StudentManagement.Services;

public class UserService(ApplicationDbContext context):IUser
{
    public async Task<User> GetUserByUsernameAsync(string username)
    {
        return await context.Users.FirstOrDefaultAsync(e=>e.Username == username)??
               throw new ApplicationException("Không tìm thấy người dùng");
    }

    public async Task AddUserAsync(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user), "Thông tin người dùng không được để trống.");

        if (string.IsNullOrWhiteSpace(user.Username))
            throw new ArgumentException("Tên người dùng không được để trống.", nameof(user.Username));
        if (!await CheckUsernameAvailabilityAsync(user.Username))
            throw new ArgumentException($"Tên người dùng '{user.Username}' đã được sử dụng.", nameof(user.Username));
        
        if (!string.IsNullOrEmpty(user.Email) && !await CheckEmailAvailabilityAsync(user.Email))
            throw new ArgumentException($"Email '{user.Email}' đã được sử dụng.", nameof(user.Email));
        
        if (string.IsNullOrWhiteSpace(user.PasswordHash))
            throw new ArgumentException("Mật khẩu không được để trống.", nameof(user.PasswordHash));
        
        if (!Enum.IsDefined(typeof(RoleEnum), user.Role))
            throw new ArgumentException($"Vai trò '{user.Role}' không hợp lệ. Vai trò phải là một trong: {string.Join(", ", Enum.GetNames(typeof(RoleEnum)))}.", nameof(user.Role));
        try
        {
            
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new ApplicationException($"Lỗi khi thêm người dùng: {ex.InnerException?.Message}", ex);
        }
    }
    public async Task<User> GetUserByEmailAsync(string email)
    {
        return  await context.Users.FirstOrDefaultAsync(e => e.Email == email)
            ??throw new ArgumentException($"Không tồn tại người dùng với Email: {email}");
    }

    public async Task UpdateUserAsync(User user)
    {
        var existing = await context.Users.FindAsync(user.Id)
                       ?? throw new ArgumentException("Không tìm thấy thông tin học sinh đã cho");
        if (existing.Email != user.Email && context.Users.Any(e => e.Email == user.Email))
            throw new ArgumentException("Email đã được sử dụng cho một tài khoản khác");
        if (!Enum.IsDefined(typeof(RoleEnum), user.Role))
            throw new ArgumentException("Role không phù hợp");
        existing.Email = user.Email;
        existing.Role = user.Role;
        existing.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
        try
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new ApplicationException(ex.Message,ex.InnerException);
        }
    }
    public async Task<bool> AuthenticateUserAsync(string username, string password)
    {
        var existing = await context.Users.FirstOrDefaultAsync(e => e.Username == username)
                       ?? throw new ApplicationException($"Không tồn tại tài khoản với username: {username}");
        
        return BCrypt.Net.BCrypt.Verify(password, existing.PasswordHash);
    }

    public async Task<bool> CheckUsernameAvailabilityAsync(string username)
    {
        return await context.Users.AnyAsync(e => e.Username == username)?false:true;
    }

    public async Task<bool> CheckEmailAvailabilityAsync(string email)
    {
        return await context.Users.AnyAsync(e => e.Email == email)?false:true;
    }
    
}