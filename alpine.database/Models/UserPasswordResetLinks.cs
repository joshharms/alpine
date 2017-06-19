using System;
using System.Collections.Generic;

namespace alpine.database.Models
{
    public partial class UserPasswordResetLinks
    {
        public Guid Id { get; set; }
        public Guid Guid { get; set; }
        public string LinkType { get; set; }
        public DateTime LinkExpiration { get; set; }
        public Guid UserId { get; set; }

        public virtual Users User { get; set; }
    }
}
