﻿using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserEmailByClaims(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
        }
    }
}