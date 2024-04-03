using Company.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace Company.PL.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name Is Required")]
        [MaxLength(50, ErrorMessage = "Max length is 50")]
        [MinLength(5, ErrorMessage = "Min length is 5")]
        public string Name { get; set; }
        [Range(22, 35, ErrorMessage = "Age Must Be In Range 22 ,35")]
        public int Age { get; set; }
        [RegularExpression("^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$", ErrorMessage = "Address Mut Be Like 123-street-city-country")]
        public string Address { get; set; }

        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        public DateTime HireDate { get; set; }

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }
        [InverseProperty("Employees")]
        public Department Department { get; set; }

        public IFormFile Image { get; set; }

        public string ImageName { get; set; }
    }



}
