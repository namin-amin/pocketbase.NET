namespace pocketbase.net.Models.Helpers;

///File contains error related models TODO not implemented
/// 

/// <summary>
/// 
/// </summary>
public abstract class BaseErrorResponse
{
    public int code { get; set; }
    public string message { get; set; } = string.Empty;
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public class Response<T> : BaseErrorResponse
    where T : class, new()
{
    public T? data { get; set; } = new();
}

/// <summary>
/// 
/// </summary>
public class Email : BaseErrorResponse
{

}

/// <summary>
/// 
/// </summary>
public class Collections : BaseErrorResponse
{

}

/// <summary>
/// 
/// </summary>
public class Password : BaseErrorResponse
{

}

/// <summary>
/// 
/// </summary>
public class Provider : BaseErrorResponse
{

}

/// <summary>
/// 
/// </summary>
public class Token : BaseErrorResponse
{

}

/// <summary>
/// 
/// </summary>
public class NewEmail : BaseErrorResponse
{

}

/// <summary>
/// 
/// </summary>
public class Title : BaseErrorResponse
{

}
