using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Services;

namespace StudentManagement.Controllers;

public class ClassInfoController(ClassInfoService classInfoService):Controller
{
    [HttpGet("Index")]
    [AuthorizeRole([RoleEnum.ADMIN])]
    public async Task<IActionResult> Index()
    {
        var classes = await classInfoService.GetClassInfosAsync();  
        return View(classes);
    }

    [HttpGet("Create")]
    [AuthorizeRole([RoleEnum.ADMIN])]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("Create")]
    [AuthorizeRole([RoleEnum.ADMIN])]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Create([FromForm] ClassInfo classInfo)
    {
        try
        {
            await classInfoService.AddClassInfoAsync(classInfo);
            return RedirectToAction("Index");
        }
        catch (ApplicationException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("Edit")]
    [AuthorizeRole([RoleEnum.ADMIN])]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var classInfo = await classInfoService.GetClassInfoByIdAsync(id);
            return View(classInfo);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("Edit")]
    [AuthorizeRole([RoleEnum.ADMIN])]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Edit([FromForm] ClassInfo classInfo)
    {
        try
        {
            await classInfoService.UpdateClassInfoAsync(classInfo);
            return RedirectToAction("Index");
        }
        catch (ApplicationException e)
        {
            return BadRequest(e.Message);
        }
    }
    // Chức năng Xóa lớp học:
    [HttpPost("Delete")]
    [AuthorizeRole([RoleEnum.ADMIN])]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await classInfoService.DeleteClassInfoAsync(id);
            return RedirectToAction("Index");
        }
        catch (ApplicationException e)
        {
            return BadRequest(e.Message);
        }
    }
    // Chức năng Nhập điểm sinh viên:
    // Chức năng Sửa điểm sinh viên:
    // Chức năng Xem điểm sinh viên:
    // Chức năng Thêm môn học:


    
    
    
    
    
    
    
}