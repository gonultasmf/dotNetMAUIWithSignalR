using System;
using dotNetWebApi.Server.Functions.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace dotNetWebApi.Server.Helpers;

public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext?.Items["User"] as User;
            if (user == null)
            {
                context.Result = new JsonResult(new { StatusMessage = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized};
            }
        }
    }
