using Domain.Entities;
using Domain.Interfaces;
using Infastracted.EF;
using Microsoft.EntityFrameworkCore;

namespace Infastracted.Data;

public class MemberRepository : IMemberRepository
{
    private readonly ProjectServiceDbContext _dbContext;

    public MemberRepository(ProjectServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Member?> GetMemberByIdAsync(string id)
    {
        return await _dbContext.Members.FirstOrDefaultAsync(m => m.Id == id);
    }
}