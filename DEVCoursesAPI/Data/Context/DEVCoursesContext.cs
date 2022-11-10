using DEVCoursesAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DEVCoursesAPI.Data.Context;

public class DEVCoursesContext: DbContext
{
    public DEVCoursesContext() { }
    
    public DEVCoursesContext(DbContextOptions<DEVCoursesContext> options) : base(options) { }
    
    public DbSet<Users> Users { get; set; }
    public DbSet<Training> Trainings { get; set; }
    public DbSet<Module> Modules { get; set; }
    public DbSet<Topic> Topics { get; set; }
}