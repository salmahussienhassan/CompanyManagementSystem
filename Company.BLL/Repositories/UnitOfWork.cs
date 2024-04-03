using Company.BLL.Interfaces;
using Company.DAL.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CompanyDbContext _companyDb;

        public IEmployeeRepository employeeRepository { get ; set ; }
        public IDepartmentRepository departmentRepository { get; set; }

        //Ask ClR make object from CompanyDbContext then send it to intialization employeeRepository and  departmentRepository
        //و بكدا بدرنا حدوث الديبيندنسي انجيكشين هنا
        public UnitOfWork(CompanyDbContext companyDb) 
          
        {
            employeeRepository = new EmployeeRepository(companyDb);
            departmentRepository = new DepartmentRepository(companyDb);
            this._companyDb = companyDb;
        }

        public async Task< int >CompleteAsync()
        {
           return await _companyDb.SaveChangesAsync();
        }

        public void Dispose()
        {
           _companyDb.Dispose();
        }
    }
}
