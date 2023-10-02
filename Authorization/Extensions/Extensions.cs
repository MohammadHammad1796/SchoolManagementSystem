namespace Authorization.Extensions;

public static class Extensions
{
	public static string ToCamelCaseName(this string input) =>
		input[0].ToString().ToLower() + input[1..];
}