using pocketbase.net.Helpers;
using pocketbase.net.Models;
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


            var val = await cleint.GetFromJsonAsync<ListModel<posts>>("", opt);

            var toshow = JsonSerializer.Serialize(val?.Items);
            richTextBox1.Text = "";
            richTextBox1.Text = toshow.ToString();

        }


        class posts : BaseModel
        {
            [JsonPropertyName("posts")]
            public string Posts { get; set; } = "";

            [JsonPropertyName("description")]
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