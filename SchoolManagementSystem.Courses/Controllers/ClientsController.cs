using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Courses.Models;
using SchoolManagementSystem.Shared.Auth;
using SchoolManagementSystem.Shared.Models;

namespace SchoolManagementSystem.Courses.Controllers;

[Route("clients")]
public class ClientsController : Controller
{
	private readonly ApplicationDbContext _dbContext;

	public ClientsController(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	[HttpGet("getRegisteredStudentsId/{specializeId}")]
	[AuthorizeService(AppServices.Attendance)]
	[Authorize(Roles = Roles.Admin)]
	public async Task<IActionResult> GetRegisteredStudentsId([FromRoute] int specializeId)
	{
		var studentIds = await _dbContext.StudentCourses.Where(sc => sc.Course.SpecializeId == specializeId).Select(sc => sc.StudentId).Distinct().ToListAsync();
		return Ok(new Result<ICollection<int>>(true, studentIds));
	}
}