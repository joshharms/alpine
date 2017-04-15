using System;
using System.Collections.Generic;

namespace alpine.database.Models
{
    public partial class Users
    {
        public Guid Id { get; set; }
        public int ClusterId { get; set; }
        public bool Active { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime PasswordLastUpdated { get; set; }
        public int AccessFailedCount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public int RoleId { get; set; }
        public int? CompanyId { get; set; }
        public Guid? Avatar { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedByUserId { get; set; }

        public virtual Roles Role { get; set; }
    }
}
