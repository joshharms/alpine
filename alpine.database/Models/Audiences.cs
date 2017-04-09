using System;
using System.Collections.Generic;

namespace alpine.database.Models
{
    public partial class Audiences
    {
        public Guid Id { get; set; }
        public int ClusterId { get; set; }
        public string Name { get; set; }
        public string Base64Secret { get; set; }
    }
}
