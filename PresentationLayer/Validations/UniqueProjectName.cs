using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using DataLayer.Entities;
using DataLayer.Data;

public class UniqueProjectName : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return new ValidationResult("Project name is required.");
        }

        var dbContext = (AppDbContext)validationContext.GetService(typeof(AppDbContext));

        // Get the project ID from the validation context object instance
        var projectIdProperty = validationContext.ObjectType.GetProperty("ProjectId");
        int projectId = (int)projectIdProperty.GetValue(validationContext.ObjectInstance);

        var projectName = value.ToString().Replace(" ", "").ToLower();

        var existingProject = dbContext.Projects
            .AsNoTracking()
            .FirstOrDefault(p => p.Name.Replace(" ", "").ToLower() == projectName && p.ProjectId != projectId);

        if (existingProject != null)
        {
            return new ValidationResult("Project name must be unique.");
        }

        return ValidationResult.Success;
    }
}
