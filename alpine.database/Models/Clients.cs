using System;
using System.Collections.Generic;

namespace alpine.database.Models
{
    public partial class Clients
    {
        public string Id { get; set; }
        public string Secret { get; set; }
        public string Name { get; set; }
        public int ApplicationType { get; set; }
        public bool Active { get; set; }
        public int RefreshTokenLifetime { get; set; }
        public string AllowedOrigin { get; set; }
    }
}
