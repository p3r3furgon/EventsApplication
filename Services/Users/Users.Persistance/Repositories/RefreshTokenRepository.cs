using Microsoft.EntityFrameworkCore;
using Users.Domain.Interfaces.Repositories;
using Users.Domain.Models.AuthModels;

namespace Users.Persistance.Repositories
{
    public class RefreshTokenRepository : IRefreshTokensRepository
    {
        private readonly UsersDbContext _context;

        public RefreshTokenRepository(UsersDbContext context)
        {
            _context = context;
        }

        public async Task Create(RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken> Get(string token)
        {
            var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);
            return refreshToken;
        }

        public async Task Save(RefreshToken refreshToken, string newRefreshToken, DateTime expirationDate)
        {
            await _context.RefreshTokens
                .Where(u => u.Id == refreshToken.Id)
                .ExecuteUpdateAsync(u => u
                .SetProperty(p => p.Token, p => newRefreshToken)
                .SetProperty(p => p.ExpirationDate, p => expirationDate));
        }
    }
}
