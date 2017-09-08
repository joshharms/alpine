using System;
namespace alpine.infrastructure.Users
{
    public class UserSummary
    {
        public UserSummary()
        {
        }

        public Guid id { get; set; }

        public bool active { get; set; }

        public string email { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string phoneNumber { get; set; }

        public string avatar { get; set; }

        public string createdDate { get; set; }
    }
}
