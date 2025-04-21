using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Services;

namespace StudentManagement.Controllers;

public class GradeController(GradeService gradeService):Controller
{
    [HttpGet("Index")]
    [AuthorizeRole([RoleEnum.ADMIN,RoleEnum.TEACHER])]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("Grade")]
    [AuthorizeRole([RoleEnum.ADMIN, RoleEnum.TEACHER])]
    public async Task<IActionResult> Grade([FromForm]int subjectId, int classId)
    {
        var collection=await gradeService.GetGradesBySubjectIdAndSemesterAsync(subjectId, classId);
        return View(collection);
    }

    [HttpPost("Create")]
    [AuthorizeRole([RoleEnum.ADMIN, RoleEnum.TEACHER])]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Create([FromForm] Grade grade)
    {
        try
        {
            await gradeService.AddGradeAsync(grade);
            return RedirectToAction("Grade");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("Update")]
    [AuthorizeRole([RoleEnum.ADMIN, RoleEnum.TEACHER])]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Update([FromForm] Grade grade)
    {
        try
        {
            await gradeService.UpdateGradeAsync(grade);
            return RedirectToAction("Grade");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{studentId}")]
    [AuthorizeRole([RoleEnum.ADMIN, RoleEnum.TEACHER, RoleEnum.STUDENT])]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> GetGrade(int studentId)
    {
        try
        {
            var result = await gradeService.GetGradesByStudentIdAsync(studentId);
            return View(result);
        }
        catch (ApplicationException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    
    
}