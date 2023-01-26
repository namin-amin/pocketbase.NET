using pocketbase.net.Helpers;
using pocketbase.net.Models.ResponseHelpers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace uitest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var cleint = new HttpClient()
            {
                BaseAddress = new Uri("http://127.0.0.1:8090/api/collections/posts/records"),


            };
            var opt = new JsonSerializerOptions();
            opt.Converters.Add(new JsonDateTimeConverter());
            opt.PropertyNamingPolicy =  JsonNamingPolicy.CamelCase;
            


            var val = await cleint.GetFromJsonAsync<ListModel>("", opt);
            //var val2 = await cleint.GetFromJsonAsync<ListModel>("", opt);

            var toshow = JsonSerializer.Serialize(val,opt);
            richTextBox1.Text = "";
            richTextBox1.Text = toshow.ToString();

        }


        class posts : BaseModel
        {
            public string Posts { get; set; } = "";
            public string Description { get; set; } = "";
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var cleint = new HttpClient()
            {
                BaseAddress = new Uri("http://127.0.0.1:8090/api/collections/posts/records"),


            };
            var opt = new JsonSerializerOptions();


            var val = await cleint.GetStringAsync("");
            richTextBox1.Text = val.ToString();

        }
    }
}