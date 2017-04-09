﻿using System;
using System.Collections.Generic;

namespace alpine.database.Models
{
    public partial class RefreshTokens
    {
        public string Id { get; set; }
        public int ClusterId { get; set; }
        public string Subject { get; set; }
        public string ClientId { get; set; }
        public DateTime IssuedUtc { get; set; }
        public DateTime ExpiresUtc { get; set; }
        public string ProtectedTicket { get; set; }
    }
}
