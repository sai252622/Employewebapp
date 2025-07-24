using Microsoft.EntityFrameworkCore;

namespace Employewebapp.Models
{
    public class Employeedbcontext : DbContext
    {
        
        public Employeedbcontext(DbContextOptions<Employeedbcontext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }

        public DbSet<CreateEmployee> Employees { get; set; }
    }

}
 