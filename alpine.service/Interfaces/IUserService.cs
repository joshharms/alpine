using System;
using System.Collections.Generic;

using alpine.infrastructure.Users;

namespace alpine.service.Interfaces
{
    public interface IUserService
    {
        List<UserSummary> GetUsers();
    }
}
