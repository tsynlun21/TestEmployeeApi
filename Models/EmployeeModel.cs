using System.ComponentModel.DataAnnotations;

namespace TestEmployee.Models;

public sealed class EmployeeModel
{
    //[Required]
    [StringLength(20, ErrorMessage = "FirstName must be at least 3 characters long.", MinimumLength = 3)]
    public string? FirstName { get; set; }
    //[Required]
    [StringLength(20, ErrorMessage = "LastName must be at least 3 characters long.", MinimumLength = 3)]
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public int? CompanyId { get; set; }
    public Passport? Passport { get; set; } = new Passport();
    public Department? Department { get; set; } = new Department();

}

[Serializable]
public  sealed class Passport
{
    public string? Type { get; set; }
    public string? Number { get; set; }

}
[Serializable]
public sealed class Department
{
    public string? Name { get; set; }
    public string? Phone { get; set; }

}