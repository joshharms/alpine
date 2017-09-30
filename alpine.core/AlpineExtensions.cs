using System;
namespace alpine.core
{
    public static class AlpineExtensions
    {
        public static string ToUtcString( this DateTime val )
        {
            if ( null != val )
            {
                return val.ToString( "yyyy-MM-ddTHH:mm:ssZ" );
            }

            return null;
        }
    }
}
