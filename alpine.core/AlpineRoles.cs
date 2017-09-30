using System;
namespace alpine.core
{
    public static class AlpineRoles
    {
        public enum RoleType
        {
            PlatformAdministrator = 1,
            Administrator = 2,
        }

        public static string ToDisplayString( this RoleType t )
        {
            switch ( t )
            {
                case RoleType.PlatformAdministrator:
                    return "Platform Administrator";
                case RoleType.Administrator:
                    return "Administrator";
            }

            return "";
        }
    }
}
