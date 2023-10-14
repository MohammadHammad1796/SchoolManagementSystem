using FluentValidation;
using SchoolManagementSystem.Admission.Controllers.Resources;

namespace SchoolManagementSystem.Admission.Controllers.Validators;

public class EnrollStudentResourceValidator : AbstractValidator<EnrollStudentResource>
{
	public EnrollStudentResourceValidator()
	{
		var thisYear = DateTime.UtcNow.Year;
		const int minimumAge = 16;
		const int maximumAge = 20;
		RuleFor(e => e.DateOfBirth)
			.NotEmpty()
			.WithMessage("Date of birth is required")
			.Must(d => YearDifference(d) >= minimumAge)
			.WithMessage($"Age should be at least {minimumAge} years")
			.Must(d => YearDifference(d) <= maximumAge)
			.WithMessage($"Age should be at maximum {maximumAge} years");

		const double minimumAverage = 150;
		const double maximumAverage = 310;
		RuleFor(e => e.CertificateAverage)
		.NotEmpty()
		.WithMessage("Certificate Average is required")
		.Must(a => a >= minimumAverage && a <= maximumAverage)
		.WithMessage($"Certificate average should be between {minimumAverage} and {maximumAverage}");

		const int minimumCertificateFrom = 0;
		const int maximumCertificateFrom = 4;
		RuleFor(e => e.CertificateDate)
			.NotEmpty()
			.WithMessage("Certificate date is required")
			.Must(d => YearDifference(d) >= minimumCertificateFrom)
			.WithMessage($"Certificate should be at least since {minimumCertificateFrom} years")
			.Must(d => YearDifference(d) <= maximumCertificateFrom)
			.WithMessage($"Certificate should be at maximum since {maximumCertificateFrom} years");
		return;

		int YearDifference(DateTime d) => thisYear - d.Year;
	}
}