using Newtonsoft.Json;

public class BaseResponse<T>
{
    public int page { get; set; }

    public int per_page { get; set; }

    public int total { get; set; }

    public int total_pages { get; set; }

    public T? data { get; set; }
}