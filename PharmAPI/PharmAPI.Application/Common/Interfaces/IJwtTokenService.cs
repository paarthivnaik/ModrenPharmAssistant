using PharmAPI.Domain.Entities;

namespace PharmAPI.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(ApplicationUser user, IList<string> roles, out DateTime expiration);
}
