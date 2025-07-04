using Microsoft.EntityFrameworkCore;

namespace Employewebapp.Models
{
    public class Employeedbcontext : DbContext
    {
        public DbSet<CreateEmployee> Employees { get; set; }
        public Employeedbcontext(DbContextOptions<Employeedbcontext> options) : base(options)
        {

        }
    }

}
 