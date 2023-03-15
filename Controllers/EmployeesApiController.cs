using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Mvc;

using TestEmployee.Interfaces;
using TestEmployee.Models;

namespace TestEmployee.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeesApiController : ControllerBase
    {
        private readonly IEmployeesService _employeesService;
        public EmployeesApiController(IEmployeesService employeesService)
        {
            _employeesService = employeesService;
        }

        [HttpPost("insertEmployee")]
        public async Task<IActionResult> InsertEmployee([FromBody] EmployeeModel employee)
        {
            try
            {
                int id = await _employeesService.AddEmployee(employee);

                return Ok(id);
            }
            catch (WebException webex)
            {
                return StatusCode(webex.HResult, webex.Message);
            }
            catch (ValidationException valex)
            {
                return StatusCode(404, valex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<string> DeleteEmployee(int id)
        {
            var response = await _employeesService.DeleteEmployee(id);
            return response
                ? $"Employee with id = {id} has been successfully deleted"
                : $"Employee with id = {id} was not found";
        }

        [HttpGet("getByCompanyName")]
        public async Task<IActionResult> GetEmployeesByCompanyName ([FromQuery] string name)
        {
            var response = await _employeesService.GetEmployesByCompany(name);

            if (response.Count > 0)
            {
                return Ok(response);
            }
            else
            {
                return NotFound($"No employees working in {name} were found");
            }
        }

        [HttpGet("getByDepartment")]
        public async Task<IActionResult> GetEmployeesByDepartment([FromQuery] Department department)
        {
            var response = await _employeesService.GetEmployesByDepartment(department);

            if (response.Count > 0)
            {
                return Ok(response);
            }
            else
            {
                return NotFound($"No employees working in {department.Name} were found");
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromQuery] EmployeeModel toChangeEmployee)
        {
            var response = await _employeesService.ChangedEmployeeById(toChangeEmployee, id);

            if (response)
            {
                return Ok();
            }
            else
            {
                return NotFound($"Employee with id = {id} was not found");
            }
        }
    }
}
