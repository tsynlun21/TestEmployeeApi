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

    public async Task<bool> ChangedEmployeeById(EmployeeModel changedModel, int id)
    {
        StringBuilder query = new StringBuilder("UPDATE Employees SET ");

        StringBuilder querySetParameters = BuildParams(changedModel); // согласно пункту в тз обновляю только те колонки, для которые были переданы значения в запросе. вторым (пожалуй более удобным) решением было бы полное обновление объекта в таблице с перезаписью всех колонок (достаем объект по id, изменяем нужные свойства и обновляем существующий в таблице)

        query.Append(querySetParameters).Append("WHERE Id = @Id");

        var affectedRows = await _dbConnection.ExecuteAsync(query.ToString(),
            new
            {
                Id = id,
                changedModel.FirstName,
                changedModel.LastName,
                changedModel.Phone,
                changedModel.CompanyId,
                PassportType = changedModel.Passport?.Type,
                PassportNumber = changedModel.Passport?.Number,
                DepartmentName = changedModel.Department?.Name,
                DepartmentPhone = changedModel.Department?.Phone
            });

        return affectedRows == 1;
    }

    private StringBuilder BuildParams(EmployeeModel changedModel)
    {
        var query = new StringBuilder();

        if (changedModel.FirstName is not null)
        {
            query.Append("FirstName = @firstName ");
        }

        if (changedModel.LastName is not null)
        {
            if (query.Length > 0)
            {
                query.Append(", ");
            }
            query.Append("LastName = @lastName ");
        }

        if (changedModel.Phone is not null)
        {
            if (query.Length > 0)
            {
                query.Append(", ");
            }
            query.Append("Phone = @phone ");
        }

        if (changedModel.CompanyId is not null)
        {
            if (query.Length > 0)
            {
                query.Append(", ");
            }
            query.Append("CompanyId = @companyId ");
        }

        if (changedModel.Passport.Number is not null)
        {
            if (query.Length > 0)
            {
                query.Append(", ");
            }
            query.Append("PassportNumber = @passportNumber ");
        }

        if (changedModel.Passport.Type is not null)
        {
            if (query.Length > 0)
            {
                query.Append(", ");
            }
            query.Append("PassportType = @passportType ");
        }

        if (changedModel.Department.Name is not null)
        {
            if (query.Length > 0)
            {
                query.Append(", ");
            }
            query.Append("DepartmentName = @departmentName ");
        }

        if (changedModel.Department.Phone is not null)
        {
            if (query.Length > 0)
            {
                query.Append(", ");
            }
            query.Append("DepartmentPhone = @departmentPhone ");
        }

        return query;
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