using System.Runtime.CompilerServices;
using pocketbase.net.Models.Helpers;
using uitest.Models;

namespace uitest.Pages
{
    public partial class Counter
    {

        public List<Posts> myposts { get; set; } = null;

        protected override async Task OnInitializedAsync()
        {
            var result = await pb.Collections("posts").GetFullList<Posts>();
            myposts = result.items.ToList();
            StateHasChanged();
        }
    }
}