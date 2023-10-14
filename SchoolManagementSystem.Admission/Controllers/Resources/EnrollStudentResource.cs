using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Admission.Controllers.Resources;

public class EnrollStudentResource
{
	[Required]
	[MaxLength(50)]
	public string FirstName { get; set; }

	[Required]
	[MaxLength(50)]
	public string LastName { get; set; }

	[Required]
	[MaxLength(100)]
	public string MotherFullName { get; set; }

	[Required]
	[MaxLength(50)]
	public string FatherFirstName { get; set; }

	[Required]
	public bool IsMale { get; set; }

	[Required]
	public DateTime DateOfBirth { get; set; }

	[Required]
	public double CertificateAverage { get; set; }

	public DateTime CertificateDate { get; set; }

	public int SpecializeId { get; set; }
}