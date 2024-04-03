using Company.BLL.Interfaces;
using Company.DAL.Contexts;
using Company.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly CompanyDbContext dbContext;

        public EmployeeRepository(CompanyDbContext dbContext) :base(dbContext) 
        {
            this.dbContext = dbContext;
        }

        public IQueryable<Employee> GetEmployeeByAddress(string address)
        {
            return dbContext.Employees.Where(e => e.Address == address);
        }
      
        public IQueryable<Employee> GetEmployeeByName(string name)
        {
            string param=name.ToLower();
            return dbContext.Employees.Where(e => e.Name.ToLower().Contains(name)).Include(e=>e.Department) ;
        }
    }
}
