using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Admission.Models;
using SchoolManagementSystem.Shared.Auth;
using SchoolManagementSystem.Shared.Extensions;
using SchoolManagementSystem.Shared.Helpers;
using SchoolManagementSystem.Shared.Models;

namespace SchoolManagementSystem.Admission.Controllers;

[Route("clients")]
public class ClientsController : Controller
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IConfiguration _configuration;
	private readonly ISchoolHttpClient _schoolHttpClient;

	public ClientsController(
		ApplicationDbContext dbContext,
		IConfiguration configuration,
		ISchoolHttpClient schoolHttpClient)
	{
		_dbContext = dbContext;
		_configuration = configuration;
		_schoolHttpClient = schoolHttpClient;
	}

	[HttpGet("isSpecializeExisted/{id}")]
	[Authorize(Roles = Roles.Admin)]
	[AuthorizeService(AppServices.Courses)]
	public async Task<IActionResult> IsSpecializeExisted([FromRoute] int id)
	{
		var specialize = await _dbContext.Specializes.SingleOrDefaultAsync(s => s.Id == id);
		if (specialize != null)
			return Ok(new Result<int>(true));

		return NotFound();
	}

	[HttpGet("getMySpecialize")]
	[AuthorizeService(AppServices.Courses)]
	[Authorize(Roles = Roles.Student)]
	public async Task<IActionResult> GetMySpecialize()
	{
		var useridResult = await _schoolHttpClient.GetResultAsync<string>($"{_configuration["AuthorizationServiceUrl"]}clients/getMyId");
		if (!useridResult.Succeeded)
			return this.ServerError();

		var userId = useridResult.ObjectResult;
		var student = await _dbContext.Students.SingleOrDefaultAsync(s => s.UserId == userId);
		if (student == null) return NotFound();

		var studentResult = new Result<int>(true, student.SpecializeId);
		return Ok(studentResult);
	}

	[HttpGet("getMyId")]
	[AuthorizeService(AppServices.Courses)]
	[Authorize(Roles = Roles.Student)]
	public async Task<IActionResult> GetMyId()
	{
		var useridResult = await _schoolHttpClient.GetResultAsync<string>($"{_configuration["AuthorizationServiceUrl"]}clients/getMyId");
		if (!useridResult.Succeeded)
			return this.ServerError();

		var userId = useridResult.ObjectResult;
		var student = await _dbContext.Students.SingleOrDefaultAsync(s => s.UserId == userId);
		if (student == null) return NotFound();

		var studentResult = new Result<int>(true, student.Id);
		return Ok(studentResult);
	}
}