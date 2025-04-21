using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.services;

namespace StudentManagement.Controllers;
[Microsoft.AspNetCore.Components.Route("Students")]
[Microsoft.AspNetCore.Components.Route("api/[controller]")]
public class StudentController(IStudentService studentService):Controller
{
    //Tất cả học sinh
    [HttpGet("Index")]
    [AuthorizeRole([RoleEnum.ADMIN])]
    public async Task<IActionResult> Index()
    {
        var students = await studentService.GetAllStudentsAsync();
        return View(students);
    }
    //Theo lớp
    
    //Form thêm sinh viên
    [HttpGet("Create")]
    [AuthorizeRole([RoleEnum.ADMIN, RoleEnum.TEACHER])]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("Create")]
    [AuthorizeRole([RoleEnum.ADMIN, RoleEnum.TEACHER])]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Student student)
    {
        if (!ModelState.IsValid)
            return View(student);
        try
        {
            await studentService.AddStudentAsync(student);
        }
        catch (Exception e)
        {
            ModelState.AddModelError("", e.Message);
            return View(student);
        }

        return RedirectToAction(nameof(Index));
    }

    // Sửa thông tin
    [HttpGet("Edit/{id}")]
    [AuthorizeRole([RoleEnum.ADMIN])] 
    public async Task<IActionResult> Edit(int id)
    {
        var student = studentService.GetStudentByStudentIdAsync(id);
        return View(student);
    }

    [HttpPost("Edit/{id}")]
    [AuthorizeRole([RoleEnum.ADMIN])]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Student student)
    {
        if(!ModelState.IsValid)
            return View(student);
        if(id != student.Id)
            return BadRequest();
        try
        {
            await studentService.UpdateStudentAsync(student);
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            ModelState.AddModelError("", e.Message);
            return View(student);
        }
    }

    [HttpPost("Delete/{id}")]
    [AuthorizeRole([RoleEnum.ADMIN])]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await studentService.DeleteStudentAsync(id);
            return RedirectToAction("Index");
        }
        catch (ApplicationException e)
        {
            TempData["Error"] = e.Message;
            return RedirectToAction("Index");
        }
    }
    //API

    [HttpGet]
    [AuthorizeRole([RoleEnum.ADMIN])]
    public async Task<IActionResult> GetAll()
    {
        var students = await studentService.GetAllStudentsAsync();
        return Ok(students);
    }

    [HttpGet("{id}")]
    [AuthorizeRole([RoleEnum.ADMIN, RoleEnum.TEACHER, RoleEnum.STUDENT])]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var student = await studentService.GetStudentByStudentIdAsync(id);
            return Ok(student);
        }
        catch (Exception e)
        {
            return NotFound();
        }
    }

    [HttpPost]
    [AuthorizeRole([RoleEnum.ADMIN])]
    public async Task<IActionResult> CreateApi([FromBody] Student student)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            await studentService.AddStudentAsync(student);
            return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
        }
        catch (ApplicationException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{id}")]
    [AuthorizeRole([RoleEnum.ADMIN])]
    public async Task<IActionResult> Update([FromBody]int id, Student student)
    {
        if(id!=student.Id)
            return BadRequest();
        try
        {
            await studentService.UpdateStudentAsync(student);
            return NoContent();
        }
        catch (ApplicationException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{id}")]
    [AuthorizeRole([RoleEnum.ADMIN])]
    public async Task<IActionResult> DeleteApi([FromBody]int id)
    {
        try
        {
            await studentService.DeleteStudentAsync(id);
            return NoContent();
        }
        catch (ApplicationException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    
}