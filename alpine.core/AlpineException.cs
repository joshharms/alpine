using System;

namespace alpine.core
{
    public class AlpineException : Exception
    {
        public AlpineException( string message )
            : base( message )
        {

        }
    }
}
