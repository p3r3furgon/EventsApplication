using Microsoft.AspNetCore.Authorization;

namespace CommonFiles.Auth.Requirements
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public RoleRequirement(string role) => Role = role;
        public string Role { get; }
    }
}
