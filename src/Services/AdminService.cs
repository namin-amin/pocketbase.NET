using pocketbase.net.Models;
using System.Net.Http;

namespace pocketbase.net.Services
{
    public class AdminService : BaseAuthService<Admin>
    {
        public AdminService(HttpClient httpClient, Pocketbase cleint) : base(httpClient, "admins", cleint)
        {
        }
    }
}
