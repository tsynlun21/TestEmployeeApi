using Dapper;
using System.Data;
using System.Text;

using TestEmployee.Interfaces;
using TestEmployee.Models;

namespace TestEmployee.Repository;

internal sealed class EmployeeRepository: IEmployeeRepository
{
    private readonly IDbConnection _dbConnection;

    public EmployeeRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<int> Add(EmployeeModel empModel)
    {
        using (_dbConnection)
        {
            var response = await _dbConnection.QueryAsync<int>(SqlConstants.INSERT_EMPLOYEE, new
            {
                empModel.FirstName,
                empModel.LastName,
                empModel.Phone,
                empModel.CompanyId,
                PassportType = empModel.Passport.Type,
                PassportNumber = empModel.Passport.Number,
                DepartmentName = empModel.Department.Name,
                DepartmentPhone = empModel.Department.Phone,
            });

            return response.Single();
        }

    }

    public async Task<bool> Delete(int id)
    {
        int rowsAffected;
        using (_dbConnection)
        {
            rowsAffected = await _dbConnection.ExecuteAsync(SqlConstants.DELETE_BY_ID, new { id });
        }

        return rowsAffected > 0;
    }

    public async Task<List<EmployeeModel>> GetEmployesByCompany(string companyName)
    {
        List<EmployeeModel> employes = new List<EmployeeModel>();

        using (_dbConnection)
        {
            var response = await _dbConnection.QueryAsync<EmployeeModel, string, string, string, string, EmployeeModel>(
                SqlConstants.SELECT_ALL + SqlConstants.WHERE_COMPANY_NAME,
                decomposeDepartmentAndPassport,
                splitOn: "PassportType,PassportNumber,DepartmentName,DepartmentPhone",
                param: new { companyName }
                );

            employes = response.ToList();
        }

        return employes;
    }

    public async Task<List<EmployeeModel>> GetEmployesByDepartment(Department department)
    {
        List<EmployeeModel> employes = new List<EmployeeModel>();

        using (_dbConnection)
        {
            var response = await _dbConnection.QueryAsync<EmployeeModel, string, string, string, string, EmployeeModel>(
                SqlConstants.SELECT_ALL + SqlConstants.WHERE_DEPARTMENT,
                decomposeDepartmentAndPassport,
                splitOn: "PassportType,PassportNumber,DepartmentName,DepartmentPhone",
                param: new { departmentName = department.Name, departmentPhone = department.Phone }
            );

            employes = response.ToList();
        }

        return employes;
    }

    public async Task<bool> ChangedEmployeeById(EmployeeModel changeModel, int id)
    {
        StringBuilder query = new StringBuilder("UPDATE Employees SET ");

        if (changeModel.FirstName is not null) query.Append("FirstName = @firstName ");
        if (changeModel.LastName is not null) query.Append(",LastName = @lastName ");
        if (changeModel.Phone is not null) query.Append(",Phone = @phone ");
        if (changeModel.CompanyId is not null) query.Append(",Phone = @phone ");
        if (changeModel.Passport.Number is not null) query.Append(",PassportNumber  = @passportNumber ");
        if (changeModel.Passport.Type is not null) query.Append(",PassportType  = @passportType ");
        if (changeModel.Passport.Type is not null) query.Append(",PassportType  = @passportType ");
        if (changeModel.Department.Name is not null) query.Append(",DepartmentName = @departmentName ");
        if (changeModel.Department.Phone is not null) query.Append(",DepartmentPhone = @departmentPhone ");

        query.Append("WHERE Id = @Id");

        var affectedRows = await _dbConnection.ExecuteAsync(query.ToString(),
            new
            {
                Id = id,
                changeModel.FirstName,
                changeModel.LastName,
                changeModel.Phone,
                changeModel.CompanyId,
                PassportType = changeModel.Passport?.Type,
                PassportNumber = changeModel.Passport?.Number,
                DepartmentName = changeModel.Department?.Name,
                DepartmentPhone = changeModel.Department?.Phone
            });

        return affectedRows == 1;
    }

    private Func<EmployeeModel, string, string, string, string, EmployeeModel> decomposeDepartmentAndPassport =
        ( employee,  passportType, passportNumber, departmentName,
            departmentPhone) =>
        {
            employee.Passport.Type = passportType;
            employee.Passport.Number = passportNumber;

            employee.Department.Name = departmentName;
            employee.Department.Phone = departmentPhone;
            return employee;
        };
}