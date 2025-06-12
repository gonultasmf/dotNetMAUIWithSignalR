using System;
using dotNetWebApi.Server.Functions.User;

namespace dotNetWebApi.Server.Helpers;

public class UserOperator
    {
        IHttpContextAccessor _httpContext;

        public UserOperator(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public User? GetRequestUser()
        {
            if (_httpContext == null)
                return null;

            return _httpContext.HttpContext?.Items["User"] as User;
        }
    }
