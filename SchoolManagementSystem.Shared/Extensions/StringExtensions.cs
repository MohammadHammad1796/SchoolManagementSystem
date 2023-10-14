namespace SchoolManagementSystem.Shared.Extensions;

public static class StringExtensions
{
	public static string ToCamelCaseName(this string input) =>
		input[0].ToString().ToLower() + input[1..];
}