using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infastracted.EF;

public class ProjectServiceDbContext : DbContext
{
    public DbSet<Project> Projects { get; set; }

    public DbSet<Member> Members { get; set; }

    public DbSet<Role> Roles { get; set; }

    public ProjectServiceDbContext(DbContextOptions<ProjectServiceDbContext> options) : base(options) { }
}
