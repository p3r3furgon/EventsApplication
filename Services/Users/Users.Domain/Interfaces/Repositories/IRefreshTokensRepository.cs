using System;
using Users.Domain.Models.AuthModels;

namespace Users.Domain.Interfaces.Repositories
{
    public interface IRefreshTokensRepository
    {
        Task Save(RefreshToken refreshToken, string newRefreshToken, DateTime expirationTime);
        Task<RefreshToken> Get(string refreshToken);
        Task Create(RefreshToken refreshToken);
    }
}
