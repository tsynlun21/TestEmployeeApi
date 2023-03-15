using TestEmployee.Models;

namespace TestEmployee.Interfaces;

public interface IEmployeeRepository
{
    internal Task<int> Add(EmployeeModel empModel);
    internal Task<bool> Delete(int id);
    internal Task<List<EmployeeModel>> GetEmployesByCompany (string companyName);
    internal Task<List<EmployeeModel>> GetEmployesByDepartment(Department department);
    internal Task<bool> ChangedEmployeeById(EmployeeModel changedModel, int id);

}