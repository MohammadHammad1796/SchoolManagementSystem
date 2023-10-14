using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Admission.Controllers.Resources;
using SchoolManagementSystem.Admission.Controllers.Validators;
using SchoolManagementSystem.Admission.Models;
using SchoolManagementSystem.Shared.Auth;
using SchoolManagementSystem.Shared.Controllers.Resources;
using SchoolManagementSystem.Shared.Extensions;
using SchoolManagementSystem.Shared.Helpers;
using System.Linq.Expressions;

namespace SchoolManagementSystem.Admission.Controllers;

[Route("registration")]
public class RegistrationController : Controller
{
	private readonly IConfiguration _configuration;
	private readonly ApplicationDbContext _dbContext;
	private readonly ISchoolHttpClient _schoolHttpClient;

	public RegistrationController(
		IConfiguration configuration,
		ApplicationDbContext dbContext,
		ISchoolHttpClient schoolHttpClient)
	{
		_configuration = configuration;
		_dbContext = dbContext;
		_schoolHttpClient = schoolHttpClient;
	}

	[HttpPost("enroll")]
	[Authorize(Roles = Roles.NoRole)]
	public async Task<IActionResult> Enroll([FromBody] EnrollStudentResource resource)
	{
		if (!this.ValidateWithFluent(new EnrollStudentResourceValidator(), resource))
			return BadRequest(ModelState);

		var specialize = await _dbContext.Specializes.SingleOrDefaultAsync(s => s.Id == resource.SpecializeId);
		if (specialize == null)
		{
			ModelState.AddModelError(nameof(resource.SpecializeId).ToCamelCaseName(), "Specialize not found");
			return BadRequest(ModelState);
		}

		var idResult = await _schoolHttpClient.GetResultAsync<string>($"{_configuration["AuthorizationServiceUrl"]}clients/getMyId");

		if (!idResult.Succeeded)
			return this.ServerError();

		var userId = idResult.ObjectResult;

		var stud = await _dbContext.Students.SingleOrDefaultAsync(s => s.UserId == userId);
		if (stud is not null)
		{
			ModelState.AddModelError("firstName", "Student already registered.");
			return BadRequest(ModelState);
		}

		var student = new Student()
		{
			CertificateAverage = resource.CertificateAverage,
			CertificateDate = resource.CertificateDate,
			DateOfBirth = resource.DateOfBirth,
			FatherFirstName = resource.FatherFirstName,
			FirstName = resource.FirstName,
			IsMale = resource.IsMale,
			LastName = resource.LastName,
			MotherFullName = resource.MotherFullName,
			SpecializeId = resource.SpecializeId,
			UserId = userId
		};

		_dbContext.Students.Add(student);
		var affectedRows = await _dbContext.SaveChangesAsync();
		if (affectedRows == 0)
			return this.ServerError();

		var changeRoleResult = await _schoolHttpClient.GetResultAsync<int>($"{_configuration["AuthorizationServiceUrl"]}clients/makeUserStudent");

		if (!changeRoleResult.Succeeded)
			return this.ServerError();

		return NoContent();
	}

	[HttpGet("getStudents")]
	[Authorize(Roles = Roles.Admin)]
	public async Task<IActionResult> GetStudents([FromQuery] StudentOptionsResource options)
	{
		IQueryable<Student> studentsQueryable = _dbContext.Students;
		int total;

		if (options != null)
		{
			if (options.Filters != null)
			{
				if (!string.IsNullOrWhiteSpace(options.Filters.Status))
					studentsQueryable = options.Filters.Status switch
					{
						"accepted" => studentsQueryable.Where(s => s.IsAccepted && s.Reviewed),
						"rejected" => studentsQueryable.Where(s => !s.IsAccepted && s.Reviewed),
						"reviewed" => studentsQueryable.Where(s => s.Reviewed),
						"notReviewed" => studentsQueryable.Where(s => !s.Reviewed),
						_ => studentsQueryable
					};

				if (options.Filters.SpecializeId.HasValue)
					studentsQueryable =
						studentsQueryable.Where(s => s.SpecializeId == options.Filters.SpecializeId.Value);

				if (options.Filters.StudentIds.Length > 0)
					studentsQueryable = studentsQueryable.Where(s => options.Filters.StudentIds.Contains(s.Id));
				else if (options.Filters.OnlyRegisterdOnCourses) return Ok(new ListWithCount<Student>());
			}

			if (options.Sort != null)
			{
				Expression<Func<Student, object>> sortExpression = options.Sort.SortBy switch
				{
					"lastName" => s => s.LastName,
					"fatherFirstName" => s => s.FatherFirstName,
					"motherFullName" => s => s.MotherFullName,
					"isMale" => s => s.IsMale,
					"dateOfBirth" => s => s.DateOfBirth,
					"specializeId" => s => s.Specialize.Name,
					"certificateDate" => s => s.CertificateDate,
					"certificateAverage" => s => s.CertificateAverage,
					"status" => s => s.IsAccepted,
					_ => s => s.FirstName
				};

				studentsQueryable = options.Sort.IsAscending ?
					studentsQueryable.OrderBy(sortExpression) :
					studentsQueryable.OrderByDescending(sortExpression);
			}

			total = studentsQueryable.Count();
			studentsQueryable = studentsQueryable
				.Skip((options.Paginate.Number - 1) * options.Paginate.Size)
				.Take(options.Paginate.Size);
		}
		else
		{
			total = studentsQueryable.Count();
			studentsQueryable = studentsQueryable
				.Take(10);
		}

		var students = await studentsQueryable.ToListAsync();
		var resource = new ListWithCount<Student>(students, total);
		return Ok(resource);
	}

	[HttpPost("admit/{id}")]
	[Authorize(Roles = Roles.Admin)]
	public async Task<IActionResult> Admit([FromRoute] int id, [FromBody] AdmitResource resource)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		var student = await _dbContext.Students.FirstOrDefaultAsync(s => s.Id == id);
		if (student == null)
			return NotFound();

		student.IsAccepted = resource.IsAccepted;
		student.Reviewed = true;

		var affectedRows = await _dbContext.SaveChangesAsync();
		if (affectedRows > 0) return Ok();

		return this.ServerError();
	}

	[HttpGet("specializes")]
	public async Task<IActionResult> GetSpecializes()
	{
		var specializes = await _dbContext.Specializes.ToListAsync();
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		return Ok(specializes);
	}

	[HttpGet("isAccepted")]
	[Authorize(Roles = Roles.Student)]
	public async Task<IActionResult> IsAccepted()
	{
		var idResult = await _schoolHttpClient.GetResultAsync<string>($"{_configuration["AuthorizationServiceUrl"]}clients/getMyId");

		if (!idResult.Succeeded)
			return this.ServerError();

		var userId = idResult.ObjectResult;

		var isAccepted = await _dbContext.Students.AnyAsync(s => s.UserId == userId && s.IsAccepted);
		return Ok(isAccepted);
	}
}