﻿using System.Text.Json.Serialization;

namespace pocketbase.net.Models.Helpers
{
    public class AuthModel : BaseModel
    {
        public string UserName { get; set; } = string.Empty;
        public bool Verified { get; set; }
        public string EmailVisibility { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
    }
}