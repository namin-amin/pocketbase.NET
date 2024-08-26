using System.Net.Http;
using pocketbase.net.Models;

namespace pocketbase.net.Services;

/// <summary>
/// Admin related operations are handled with this
/// has methods and properties to set and authenticate a Admin user
/// </summary>
public class AdminService : BaseAuthService<Admin>
{
    public AdminService(HttpClient httpClient, Pocketbase client) : base(httpClient, "admins", client)
    {
    }
}
