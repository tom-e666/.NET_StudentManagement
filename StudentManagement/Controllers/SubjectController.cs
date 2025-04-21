using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Services;

namespace StudentManagement.Controllers;

public class SubjectController(SubjectService subjectService):Controller
{
    [HttpGet("Index")]
    [AuthorizeRole([RoleEnum.ADMIN, RoleEnum.TEACHER])]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Index()
    {
        try
        {
            var result = await subjectService.GetSubjectsAsync();
            return View(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("Add")]
    [AuthorizeRole([RoleEnum.ADMIN, RoleEnum.TEACHER])]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost("Add")]
    [AuthorizeRole([RoleEnum.ADMIN, RoleEnum.TEACHER])]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Add([FromForm] Subject subject)
    {
        try
        {
            await subjectService.AddSubjectAsync(subject);
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("Delete")]
    [AuthorizeRole([RoleEnum.ADMIN, RoleEnum.TEACHER])]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Delete([FromForm] int id)
    {
        try
        {
            await subjectService.DeleteSubjectAsync(id);
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}