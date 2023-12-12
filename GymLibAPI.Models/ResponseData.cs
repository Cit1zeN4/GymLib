namespace GymLibAPI.Models;

public class ResponseData<T>
{
    public int TotalCount { get; set; }
    public List<T> Records { get; set; }
}