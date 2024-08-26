namespace pocketbase.net.Store;

public class BaseAuthStore
{
    public event EventHandler? Onchange;
    private string _token = "";
    private dynamic _model = default!;

    public bool isValid
    {
        get; private set;
    }

    public string token
    {
        get { return _token; }
        internal set { _token = value; }
    }

    public dynamic model
    {
        get { return _model; }
        internal set
        {
            _model = value;
        }
    }

    internal void Clear(Action<object, EventArgs>? callback = null)
    {
        _token = "";
        isValid = false;
        //?How to format the eventargs?
        callback?.Invoke(this, EventArgs.Empty);
        Onchange?.Invoke(this, EventArgs.Empty);
    }

}
