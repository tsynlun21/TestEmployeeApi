namespace TestEmployee.Repository;

internal static class SqlConstants
{
    public const string INSERT_EMPLOYEE =
        "INSERT INTO [Employees] (FirstName, LastName, Phone, CompanyId, PassportType, PassportNumber, DepartmentName, DepartmentPhone) VALUES (@FirstName, @LastName, @Phone, @CompanyId, @PassportType, @PassportNumber, @DepartmentName, @DepartmentPhone); SELECT CAST(SCOPE_IDENTITY() as int)";

    public const string DELETE_BY_ID = "DELETE FROM[dbo].[Employees] WHERE[Id] = @id";

    public const string SELECT_ALL = "SELECT * FROM [dbo].[Employees]";

    public const string WHERE_COMPANY_NAME = "WHERE CompanyId = @CompanyName";

    public const string WHERE_DEPARTMENT = "WHERE DepartmentName = @departmentName AND DepartmentPhone = @departmentPhone";

}