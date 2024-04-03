using AutoMapper;
using Company.DAL.Models;
using Company.PL.ViewModels;

namespace Company.PL.MappingProfiles
{
    public class EmployeeProfile:Profile
    {
        //to make _mapper understand binding props from EmployeeViewModel to employee so make configuration here
        //every mapper has profile
        //Handel configuring here if prop in EmployeeViewModel has different name from its corresponding in employee
        public EmployeeProfile() 
        { 
            //d=> distination,s=> source,o=> option
        // CreateMap<EmployeeViewModel,Employee>().ForMember(d=>d.Name,o=>o.MapFrom(s=>s.EmpName));
         CreateMap<EmployeeViewModel,Employee>().ReverseMap();
        }
    }
}
