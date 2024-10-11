using Microsoft.AspNetCore.Authorization;

namespace Users.API.Authorization.Requirements
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public RoleRequirement(string role) => Role = role;
        public string Role { get; }
    }
}
