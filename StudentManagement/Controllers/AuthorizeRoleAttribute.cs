using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StudentManagement.Models;

namespace StudentManagement.Controllers;

public class AuthorizeRoleAttribute(RoleEnum[] allowedRoles):ActionFilterAttribute
{
    public void OnActionExecuted(ActionExecutingContext context)
    {
        var role=context.HttpContext.Session.GetString("role");
        if(string.IsNullOrEmpty(role)|| !Enum.TryParse(role,out RoleEnum _role) || !allowedRoles.Contains(_role))
            context.Result= new RedirectToActionResult("Login","Account",null);
        base.OnActionExecuting(context);
    }
    
}   