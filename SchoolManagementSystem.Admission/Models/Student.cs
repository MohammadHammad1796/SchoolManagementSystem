using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Admission.Models;

public class Student
{
	public int Id { get; set; }

	[Required]
	public string FirstName { get; set; }

	[Required]
	public string LastName { get; set; }

	[Required]
	public string MotherFullName { get; set; }

	[Required]
	public string FatherFirstName { get; set; }

	public bool IsMale { get; set; }

	public DateTime DateOfBirth { get; set; }

	public double CertificateAverage { get; set; }

	public DateTime CertificateDate { get; set; }

	public bool IsAccepted { get; set; }

	public bool Reviewed { get; set; }

	public int SpecializeId { get; set; }

	public Specialize Specialize { get; set; }

	[Required]
	public string UserId { get; set; }
}