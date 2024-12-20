﻿using System.Security.Claims;
using Users.Domain.Models;

namespace Users.Infrastructure
{
    public interface IJwtProvider
    {
        string GenerateJwtToken(User user);
        string GenerateRefreshToken();
    }
}