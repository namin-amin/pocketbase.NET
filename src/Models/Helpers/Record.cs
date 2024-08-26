namespace pocketbase.net.Models.Helpers;

/// <summary>
/// Record is class representing the returned data
/// </summary>
/// <typeparam name="T">type of items</typeparam>
public class Record<T> : BaseRecord
    where T : PbBaseModel
{
    public int totalPages { get; init; }

    public IEnumerable<T> items { get; init; } = new List<T>();
}

/// <summary>
/// Same as Record but used for AuthAdminModel and Adminmodel
/// </summary>
/// <typeparam name="T"></typeparam>
public class ColRecord<T> : BaseRecord
    where T : BaseModel
{
    public int totalPages { get; init; }

    public IEnumerable<T> items { get; init; } = new List<T>();
}

public class Record : BaseRecord
{
    public int totalPages { get; set; }
    public IEnumerable<IDictionary<string, object>> items { get; set; } = new List<Dictionary<string, object>>();
}

public class AdminRecord : BaseRecord
{
    public IEnumerable<RecordAuthModel> items { get; init; } = new List<RecordAuthModel>();
}
public abstract class BaseRecord
{
    public int page { get; set; }
    public int perPage { get; set; }
    public int totalItems { get; set; }
}

