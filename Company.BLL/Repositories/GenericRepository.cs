using Company.BLL.Interfaces;
using Company.DAL.Contexts;
using Company.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly CompanyDbContext dbContext;

        public GenericRepository(CompanyDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }
        public async Task AddAsync(T item)
           => await dbContext.AddAsync(item);
          
        

        public void Delete(T item)
  
         =>  dbContext.Remove(item);

        public async Task< IEnumerable<T>> GetAllAsync()
        {
            if(typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>) await dbContext.Employees.Include(e=>e.Department).ToListAsync();
            }
            return await dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            //    var department= _dbContext.Departments.Local.Where(d=>d.Id == id).FirstOrDefault();
            //    if (department == null)
            //        department = _dbContext.Departments.Where(d => d.Id == id).FirstOrDefault();

            return await dbContext.FindAsync<T>(id);
        }

        public void Update(T item)
          =>  dbContext.Update(item);

    }
}
