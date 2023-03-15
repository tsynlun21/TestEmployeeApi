using TestEmployee.Models;

namespace TestEmployee.Interfaces;

public interface IEmployeesService
{
    internal Task<int> AddEmployee(EmployeeModel empModel);
    internal Task<bool> DeleteEmployee(int id);
    internal Task<List<EmployeeModel>> GetEmployesByCompany(string companyName);
    internal Task<List<EmployeeModel>> GetEmployesByDepartment(Department department);
    internal Task<bool> ChangedEmployeeById(EmployeeModel changeModel, int id);
}