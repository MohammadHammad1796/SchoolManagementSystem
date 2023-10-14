namespace SchoolManagementSystem.Shared.Controllers.Resources;

public class ListWithCount<T>
{
	public ICollection<T> Data { get; set; } = new List<T>();

	public int Total { get; set; }

	public ListWithCount(ICollection<T> data, int total)
	{
		Data = data;
		Total = total;
	}

	public ListWithCount()
	{
	}
}