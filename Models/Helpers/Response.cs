using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace pocketbase.net.Models.Helpers
{

    public abstract class BaseErrorResponse
    {
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class Response<T> : BaseErrorResponse
        where T : class, new()
    {
        public T? Data { get; set; } = new();
    }

    public class Email : BaseErrorResponse
    {

    }

    public class Collections : BaseErrorResponse
    {

    }
    public class Password : BaseErrorResponse
    {

    }

    public class Provider : BaseErrorResponse
    {

    }
    public class Token : BaseErrorResponse
    {

    }
    public class NewEmail : BaseErrorResponse
    {

    }

    public class Title : BaseErrorResponse
    {

    }
}
