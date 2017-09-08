using System;

namespace alpine.core
{
    public class AuthenticationToken
    {
        public AuthenticationToken()
        {

        }

        public Guid userId { get; set; }

        public string email { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string role { get; set; }

        public string client { get; set; }

        public Guid organizationId { get; set; }

        public string avatar { get; set; }

        public string issuer { get; set; }

        public string audience { get; set; }

        public string exp { get; set; }

        public string nbf { get; set; }
    }
}
