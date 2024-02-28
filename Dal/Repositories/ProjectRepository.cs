using Dal.EF;
using Dal.Entities;
using Dal.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dal.Repositories;

public class ProjectRepository : IRepository<ProjectDal, int>
{
    private readonly TaskManagerDbContext _dbContext;

    public ProjectRepository(TaskManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateAsync(ProjectDal item)
    {
        await _dbContext.Projects.AddAsync(item);
    }

    public void Delete(ProjectDal item)
    {
        _dbContext.Projects.Remove(item);
    }

    public IEnumerable<ProjectDal> GetAll()
    {
        return _dbContext.Projects;
    }

    public async Task<ProjectDal?> GetByIdAsync(int id)
    {
        return await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id);
    }

    public void Update(ProjectDal item)
    {
        _dbContext.Entry(item).State = EntityState.Modified;
    }
}
