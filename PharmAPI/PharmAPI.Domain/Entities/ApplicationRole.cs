using Microsoft.AspNetCore.Identity;

namespace PharmAPI.Domain.Entities;

public class ApplicationRole : IdentityRole
{
    public ApplicationRole() : base()
    {
    }

    public ApplicationRole(string roleName) : base(roleName)
    {
    }
}
