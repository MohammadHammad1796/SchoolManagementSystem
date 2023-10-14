using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Admission.Models;

public class Specialize
{
	public int Id { get; set; }

	[Required]
	public string Name { get; set; }
}