using System;
using System.Security.Claims;

namespace EcomTemplate.API.HelperFunctions
{
    public static class UserHelper
    {
        public static Guid GetUserId(ClaimsPrincipal user)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User not authenticated");

            return Guid.Parse(userId);
        }
    }
}

