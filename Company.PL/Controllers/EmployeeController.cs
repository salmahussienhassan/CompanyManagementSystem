using AutoMapper;
using Company.BLL.Interfaces;
using Company.DAL.Models;
using Company.PL.Helper;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //use it to make every object come or go to view from EmployeeViewModel
        //and every object come or go to database from employee
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }
        public async Task< IActionResult> Index(string name)
        {
            IEnumerable<Employee> Employees;

            if (string.IsNullOrEmpty(name))
                 Employees = await  _unitOfWork.employeeRepository.GetAllAsync();

            else
                Employees = _unitOfWork.employeeRepository.GetEmployeeByName(name);

            var MappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(Employees);

            return View(MappedEmployees);
            
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Departments=await _unitOfWork.departmentRepository.GetAllAsync();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
               var ImageName= DocumentSetting.UploadFile(employeeVM.Image, "Images");
                employeeVM.ImageName = ImageName;


                var MappedEmployee=_mapper.Map<EmployeeViewModel,Employee>(employeeVM);
               await _unitOfWork.employeeRepository.AddAsync(MappedEmployee);
               await _unitOfWork.CompleteAsync();
                
                return RedirectToAction(nameof(Index));
            }
            return View(employeeVM);
        }

        public async Task<IActionResult> Details(int? id,string ViewName="Details")
        {
            if (id is null)
                return BadRequest();
            var emp = await _unitOfWork.employeeRepository.GetByIdAsync(id.Value);
           
            if(emp is null)
                return NotFound();

            var MappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(emp);
            return View(MappedEmployee);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            //ViewBag.Departments = departmentRepository.GetAll();
            return await Details(id,"Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<  IActionResult> Edit(EmployeeViewModel employeeVM,int id)
        {
           if(id != employeeVM.Id)
                BadRequest();
           if(ModelState.IsValid)
            
                try
                {
                    var ImageName = DocumentSetting.UploadFile(employeeVM.Image, "Images");
                    employeeVM.ImageName = ImageName;

                    var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _unitOfWork.employeeRepository.Update(MappedEmployee);
                   var result= await _unitOfWork.CompleteAsync();

                    return  RedirectToAction(nameof(Index));
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError(string.Empty,ex.Message);
                }
            return View(employeeVM);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");      
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(EmployeeViewModel employeeVM,int id)
        {
            if(employeeVM.Id !=id)
                return BadRequest();
            if(ModelState.IsValid)
                try
                {
                    var MappedEmployee= _mapper.Map<EmployeeViewModel,Employee>(employeeVM);
                    _unitOfWork.employeeRepository.Delete(MappedEmployee);
                  var Result= await  _unitOfWork.CompleteAsync();
                    if (Result > 0 && employeeVM.ImageName is not null)
                        DocumentSetting.DeleteFile(employeeVM.ImageName, "Images");
                    return RedirectToAction(nameof(Index));
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            return View(employeeVM);
        }

       
       



    }

}
