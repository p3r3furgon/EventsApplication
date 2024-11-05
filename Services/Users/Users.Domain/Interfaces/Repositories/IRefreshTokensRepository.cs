using System;
using Users.Domain.Models.AuthModels;

namespace Users.Domain.Interfaces.Repositories
{
    public interface IRefreshTokensRepository
    {
        Task<RefreshToken> Get(string refreshToken);
        Task Create(RefreshToken refreshToken);
    }
}
