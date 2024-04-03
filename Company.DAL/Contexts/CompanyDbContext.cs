using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Company.DAL.Contexts
{
    public class CompanyDbContext : IdentityDbContext<ApplicationUer>


    {
       public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        //   => optionsBuilder.UseSqlServer("Server=. ; Database=CompanyMvc; Trusted-Connection=true;");


       public DbSet<Department>Departments {get; set;}
        public DbSet<Employee> Employees { get; set; }
    }
}
