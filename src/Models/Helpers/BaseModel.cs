namespace pocketbase.net.Models.Helpers
{
    public abstract class BaseModel
    {
        public string id { get; set; } = string.Empty;
        public DateTime created { get; set; }
        public DateTime updated { get; set; }

        public override string ToString()
        {
            return Serialize(MemberwiseClone());
        }
    }
}
