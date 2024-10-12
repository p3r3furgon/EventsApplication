using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Users.Domain.Interfaces.Repositories;
using Users.Domain.Models.AuthModels;
using Users.Persistance.Entities;

namespace Users.Persistance.Repositories
{
    public class RefreshTokenRepository : IRefreshTokensRepository
    {
        private readonly UsersDbContext _context;
        private readonly IMapper _mapper;
        public RefreshTokenRepository(UsersDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Create(RefreshToken refreshToken)
        {
            var refreshTokenEntity = _mapper.Map<RefreshTokenEntity>(refreshToken);
            await _context.RefreshTokens.AddAsync(refreshTokenEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken> Get(string refreshToken)
        {
            var refreshTokenEntity = await _context.RefreshTokens.SingleOrDefaultAsync(rt => rt.Token == refreshToken);
            return _mapper.Map<RefreshToken>(refreshTokenEntity);
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
