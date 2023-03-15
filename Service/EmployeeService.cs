using TestEmployee.Interfaces;
using TestEmployee.Models;

namespace TestEmployee.Service;

internal sealed class EmployeeService : IEmployeesService
{
    private readonly IEmployeeRepository _repository;

    public EmployeeService(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> AddEmployee(EmployeeModel empModel)
    {
        return await _repository.Add(empModel);
    }

    public async Task<bool> DeleteEmployee(int id)
    {
        return await _repository.Delete(id);
    }

    public async Task<List<EmployeeModel>> GetEmployesByCompany(string companyName)
    {
        return await _repository.GetEmployesByCompany(companyName);
    }

    public async Task<List<EmployeeModel>> GetEmployesByDepartment(Department department)
    {
        return await _repository.GetEmployesByDepartment(department);
    }

    public async Task<bool> ChangedEmployeeById(EmployeeModel changeModel, int id)
    {
        return await _repository.ChangedEmployeeById(changeModel, id);
    }
}