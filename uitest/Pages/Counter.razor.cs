using System.Runtime.CompilerServices;
using pocketbase.net.Models;
using pocketbase.net.Models.Helpers;
using uitest.Models;

namespace uitest.Pages
{
    public partial class Counter
    {

        public List<Posts> myposts { get; set; } = null;

        //protected override async Task OnInitializedAsync()
        //{
        //    var result = await pb.Collections("todo").GetFullList<Posts>();
        //    Console.WriteLine(result.items.ToList()[0].ToString());
        //    myposts = result.items.ToList();
        //    StateHasChanged();
        //}

        public void sub()
        {
            pb.Collections("todo").Subscribe("todo", dothing);
            // pb.Collections("").
        }

        private void dothing(RealtimeEventArgs obj)
        {
            Console.WriteLine(obj.ToString());
            Console.WriteLine(obj.data.action);
        }
    }
}