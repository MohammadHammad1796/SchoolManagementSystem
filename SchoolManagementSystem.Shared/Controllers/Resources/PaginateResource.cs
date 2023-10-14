using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Shared.Controllers.Resources;

public class PaginateResource
{
	[Range(1, int.MaxValue)]
	public int Number { get; set; } = 1;

	[Range(1, 100)]
	public int Size { get; set; } = 10;
}