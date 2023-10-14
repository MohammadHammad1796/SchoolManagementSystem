using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Courses.Controllers.Resources;
using SchoolManagementSystem.Courses.Models;
using SchoolManagementSystem.Shared.Auth;
using SchoolManagementSystem.Shared.Controllers.Resources;
using SchoolManagementSystem.Shared.Extensions;
using SchoolManagementSystem.Shared.Helpers;

namespace SchoolManagementSystem.Courses.Controllers;

[Route("courses")]
public class CoursesController : Controller
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IConfiguration _configuration;
	private readonly ISchoolHttpClient _schoolHttpClient;

	public CoursesController(
		ApplicationDbContext dbContext,
		IConfiguration configuration,
		ISchoolHttpClient schoolHttpClient)
	{
		_dbContext = dbContext;
		_configuration = configuration;
		_schoolHttpClient = schoolHttpClient;
	}

	[Authorize(Roles = Roles.Admin)]
	[HttpPost("add")]
	public async Task<IActionResult> Add([FromBody] Course course)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		var courseWithSameName = await _dbContext.Courses.SingleOrDefaultAsync(c => c.Name == course.Name && c.SpecializeId == course.SpecializeId);
		if (courseWithSameName != null)
		{
			ModelState.AddModelError(nameof(courseWithSameName.Name).ToCamelCaseName(), "Course name duplicated in that specialize.");
			return BadRequest(ModelState);
		}

		var specializeExistedResult = await _schoolHttpClient.GetResultAsync<int>($"{_configuration["AdmissionServiceUrl"]}clients/isSpecializeExisted/{course.SpecializeId}");
		if (!specializeExistedResult.Succeeded)
			return this.ServerError();

		_dbContext.Courses.Add(course);
		await _dbContext.SaveChangesAsync();
		return Ok();
	}

	[Authorize(Roles = Roles.Admin)]
	[HttpPut("update/{id}")]
	public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Course resource)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		var course = await _dbContext.Courses.SingleOrDefaultAsync(c => c.Id == id);
		if (course == null)
			return NotFound();

		var courseWithSameName = await _dbContext.Courses.SingleOrDefaultAsync(c => c.Id != id && c.Name == resource.Name && c.SpecializeId == resource.SpecializeId);
		if (courseWithSameName != null)
		{
			ModelState.AddModelError(nameof(courseWithSameName.Name).ToCamelCaseName(), "Course name duplicated in that specialize.");
			return BadRequest(ModelState);
		}

		var specializeExistedResult = await _schoolHttpClient.GetResultAsync<int>($"{_configuration["AdmissionServiceUrl"]}clients/isSpecializeExisted/{course.SpecializeId}");
		if (!specializeExistedResult.Succeeded)
			return this.ServerError();

		course.Name = resource.Name;
		course.SpecializeId = resource.SpecializeId;
		await _dbContext.SaveChangesAsync();
		return Ok();
	}

	[Authorize(Roles = Roles.Admin)]
	[HttpDelete("delete/{id}")]
	public async Task<IActionResult> Delete([FromRoute] int id)
	{
		var course = await _dbContext.Courses.SingleOrDefaultAsync(c => c.Id == id);
		if (course == null)
			return NoContent();

		_dbContext.Courses.Remove(course);
		await _dbContext.SaveChangesAsync();
		return NoContent();
	}

	[Authorize(Roles = Roles.Admin)]
	[HttpGet("getAll")]
	public async Task<IActionResult> GetAll([FromQuery] CourseOptionsResource options)
	{
		IQueryable<Course> coursesQueryable = _dbContext.Courses;
		int total;

		if (options != null)
		{
			if (options.Filters?.SpecializeId != null)
				coursesQueryable =
					coursesQueryable.Where(s => s.SpecializeId == options.Filters.SpecializeId.Value);

			if (options.Sort != null)
			{
				coursesQueryable = options.Sort.IsAscending ?
					coursesQueryable.OrderBy(s => s.Name) :
					coursesQueryable.OrderByDescending(s => s.Name);
			}

			total = coursesQueryable.Count();
			coursesQueryable = coursesQueryable
				.Skip((options.Paginate.Number - 1) * options.Paginate.Size)
				.Take(options.Paginate.Size);
		}
		else
		{
			total = coursesQueryable.Count();
			coursesQueryable = coursesQueryable
				.Take(10);
		}

		var courses = await coursesQueryable.ToListAsync();
		var resource = new ListWithCount<Course>(courses, total);
		return Ok(resource);
	}

	[Authorize(Roles = Roles.Admin)]
	[HttpGet("getById/{id}")]
	public async Task<IActionResult> GetById([FromRoute] int id)
	{
		var course = await _dbContext.Courses.SingleOrDefaultAsync(c => c.Id == id);
		if (course == null)
			return NotFound();

		return Ok(course);
	}

	[Authorize(Roles = Roles.Student)]
	[HttpPost("register")]
	public async Task<IActionResult> Register([FromBody] IEnumerable<int> courseIds)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);

		var studentIdResult = await _schoolHttpClient.GetResultAsync<int>($"{_configuration["AdmissionServiceUrl"]}clients/getMyId");
		if (!studentIdResult.Succeeded)
			return this.ServerError();

		var studentId = studentIdResult.ObjectResult;

		courseIds = courseIds.Distinct().ToList();
		var studentCourses = courseIds.Select(c => new StudentCourse { CourseId = c, StudentId = studentId });
		await _dbContext.StudentCourses.AddRangeAsync(studentCourses);

		await _dbContext.SaveChangesAsync();
		return Ok();
	}

	[Authorize(Roles = Roles.Student)]
	[HttpGet("getForMySpecialize")]
	public async Task<IActionResult> GetForMySpecialize()
	{
		var studentIdResult = await _schoolHttpClient.GetResultAsync<int>($"{_configuration["AdmissionServiceUrl"]}clients/getMySpecialize");
		if (!studentIdResult.Succeeded)
			return this.ServerError();

		var specializeId = studentIdResult.ObjectResult;

		var courses = await _dbContext.Courses.Where(c => c.SpecializeId == specializeId).ToListAsync();
		return Ok(courses);
	}

	[Authorize(Roles = Roles.Student)]
	[HttpGet("isRegistered")]
	public async Task<IActionResult> IsRegistered()
	{
		var studentIdResult = await _schoolHttpClient.GetResultAsync<int>($"{_configuration["AdmissionServiceUrl"]}clients/getMyId");
		if (!studentIdResult.Succeeded)
			return this.ServerError();

		var studentId = studentIdResult.ObjectResult;

		var isCoursesRegistered = await _dbContext.StudentCourses.AnyAsync(sc => sc.StudentId == studentId);
		return Ok(isCoursesRegistered);
	}

	[Authorize(Roles = Roles.Student)]
	[HttpGet("getMine")]
	public async Task<IActionResult> GetMine()
	{
		var studentIdResult = await _schoolHttpClient.GetResultAsync<int>($"{_configuration["AdmissionServiceUrl"]}clients/getMyId");
		if (!studentIdResult.Succeeded)
			return this.ServerError();

		var studentId = studentIdResult.ObjectResult;

		var courses = await _dbContext.StudentCourses.Where(sc => sc.StudentId == studentId).Select(sc => sc.Course).ToListAsync();
		return Ok(courses);
	}

	[Authorize(Roles = Roles.Admin)]
	[HttpGet("getStudentCourses/{studentId}")]
	public async Task<IActionResult> GetStudentCourses(int studentId)
	{
		var courses = await _dbContext.StudentCourses.Where(sc => sc.StudentId == studentId).Select(sc => sc.Course).ToListAsync();
		return Ok(courses);
	}
}