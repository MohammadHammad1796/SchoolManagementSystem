using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Attendance.Controllers.Resources;
using SchoolManagementSystem.Attendance.Models;
using SchoolManagementSystem.Shared.Auth;
using SchoolManagementSystem.Shared.Controllers.Resources;
using SchoolManagementSystem.Shared.Extensions;
using SchoolManagementSystem.Shared.Helpers;

namespace SchoolManagementSystem.Attendance.Controllers;

[Route("attend")]
public class AttendController : Controller
{
	private readonly IConfiguration _configuration;
	private readonly ApplicationDbContext _dbContext;
	private readonly ISchoolHttpClient _schoolHttpClient;

	public AttendController(
		IConfiguration configuration,
		ApplicationDbContext dbContext,
		ISchoolHttpClient schoolHttpClient)
	{
		_configuration = configuration;
		_dbContext = dbContext;
		_schoolHttpClient = schoolHttpClient;
	}

	[HttpGet("getStudents")]
	[Authorize(Roles = Roles.Admin)]
	public async Task<IActionResult> GetStudents([FromQuery] AttendanceOptionsResource options)
	{
		if (!ModelState.IsValid || options?.Filters is null)
			return BadRequest(ModelState);

		var studentIdsResult = await _schoolHttpClient.GetResultAsync<ICollection<int>>($"{_configuration["CoursesServiceUrl"]}clients/getRegisteredStudentsId/{options.Filters.SpecializeId}");
		if (!studentIdsResult.Succeeded)
			return this.ServerError();

		if (studentIdsResult.ObjectResult.Count == 0) return Ok(new ListWithCount<StudentAttendanceResource>());

		var studentIds = studentIdsResult.ObjectResult;

		var query = $"options.paginate.number={options.Paginate.Number}&options.paginate.size={options.Paginate.Size}";
		if (options.Sort is not null)
			query += $"&options.sort.sortBy={options.Sort.SortBy}&options.sort.isAscending={options.Sort.IsAscending}";
		query += $"&options.filters.status=accepted&options.filters.specializeId={options.Filters!.SpecializeId}";
		query += "&options.filters.onlyRegisterdOnCourses=true";
		query = studentIds.Aggregate(query, (current, studentId) => current + $"&options.filters.StudentIds={studentId}");

		var studentsResult = await _schoolHttpClient.GetAsync<ListWithCount<StudentAttendanceResource>>($"{_configuration["AdmissionServiceUrl"]}registration/getStudents?{query}");
		if (studentsResult is null)
			return this.ServerError();

		var studentsAttendances = await _dbContext.StudentAttendances.Where(sa => studentIds.Contains(sa.StudentId) && sa.AttendanceDate.Date.Equals(options.Filters.Date)).ToDictionaryAsync(sa => sa.StudentId, sa => sa.IsAttended);
		for (var i = 0; i < studentsResult.Data.Count; i++)
		{
			var studentAttendanceResource = studentsResult.Data.ElementAt(i);
			if (studentsAttendances.TryGetValue(studentAttendanceResource.Id, out var isAttended))
				studentAttendanceResource.IsAttended = isAttended;
			else
				studentAttendanceResource.IsAttended = null;
		}

		return Ok(studentsResult);
	}

	[HttpPost("saveAttendance")]
	[Authorize(Roles = Roles.Admin)]
	public async Task<IActionResult> SaveAttendance([FromBody] AttendResource resource)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		var date = await _dbContext.AttendanceDates.SingleOrDefaultAsync(ad => ad.Date.Equals(resource.Date));
		if (date is null)
		{
			date = new AttendanceDate()
			{
				Date = resource.Date
			};
			await _dbContext.AttendanceDates.AddAsync(date);
		}

		var studentAttendance = await _dbContext.StudentAttendances.SingleOrDefaultAsync(sa =>
			sa.AttendanceDateId == date.Id && sa.StudentId == resource.StudentId);
		if (studentAttendance is null)
		{
			studentAttendance = new StudentAttendance()
			{
				AttendanceDate = date,
				StudentId = resource.StudentId,
				IsAttended = resource.IsAttended,
			};
			await _dbContext.StudentAttendances.AddAsync(studentAttendance);
		}
		else
		{
			studentAttendance.IsAttended = resource.IsAttended;
		}

		var affectedRows = await _dbContext.SaveChangesAsync();
		if (affectedRows > 0) return Ok();

		return this.ServerError();
	}
}