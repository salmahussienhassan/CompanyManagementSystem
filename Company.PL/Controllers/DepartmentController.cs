using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {

        //public DepartmentController(IDepartmentRepository departmentRepository) //Ask CLR create object from class implement IDepartmentRepository
        //{
        //    _departmentRepository = departmentRepository;
        //}

       
        //////////////////////////// after using work of uint
        

        private readonly IUnitOfWork _unitOfWork;
        public DepartmentController(IUnitOfWork unitOfWork) 
        {
            
            this._unitOfWork = unitOfWork;
        }

        public async Task< IActionResult> Index()
        {
            var departments = await _unitOfWork.departmentRepository.GetAllAsync();

            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();

        }
        [HttpPost]
        public async Task< IActionResult> Create(Department department)
        {
            if (ModelState.IsValid) //server side validation
            {
             await  _unitOfWork.departmentRepository.AddAsync(department);
                var Result = await _unitOfWork.CompleteAsync();

                if (Result>0)
                {
                    TempData["Message"] = $"{department.Name} Department is Created Successfully";
                }
                return RedirectToAction(nameof(Index));
            }

            return View(department);

        }

        public async Task< IActionResult> Details(int? id, String ViewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var department = await _unitOfWork.departmentRepository.GetByIdAsync(id.Value);

            if (department is null)
                return NotFound();

            return View(ViewName, department);
        }

        [HttpGet]

        public async Task<IActionResult> Edit(int? id)
        {
            //    if (id is null)
            //        return BadRequest();
            //    var department = _departmentRepository.GetById(id.Value);
            //    if (department is null)
            //        return NotFound();

            //    return View(department);

            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult >Edit(Department department, [FromRoute] int id)
        {
            if (id != department.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.departmentRepository.Update(department);
                   await _unitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);

                }

            }

            return View(department);

        }

   
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }


        [HttpPost]
        public async Task< IActionResult> Delete(Department department, [FromRoute] int id)
        {
            if (id != department.Id)
                return BadRequest();

            if(ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.departmentRepository.Delete(department);
                    await _unitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index)); 
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(department);
        }


    }
}
