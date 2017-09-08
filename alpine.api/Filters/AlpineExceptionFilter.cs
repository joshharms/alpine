using System;
using System.Net;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using alpine.core;

namespace alpine.api.Filters
{
    public class AlpineExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException( ExceptionContext context )
        {
            Exception exception = context.Exception;
            string message = exception.Message;

            Exception ex = context.Exception;

            //Get Inner Most Exeption
            while ( ex.InnerException != null )
            {
                ex = ex.InnerException;
                message = ex.Message;
            }

            var errorData = new { success = false, message = "An error has occured.", extraMessage = message, alpine = false };

            if ( exception is AlpineException )
            {
                errorData = new { success = false, message = exception.Message, extraMessage = "", alpine = ( exception is AlpineException ) };
            }

            if ( exception is UnauthorizedAccessException )
            {
                context.HttpContext.Response.StatusCode = ( int )HttpStatusCode.Unauthorized;
            }
            else
            {
                context.HttpContext.Response.StatusCode = ( int )HttpStatusCode.BadRequest;
            }


            context.Result = new JsonResult( errorData );

            base.OnException( context );
        }
    }
}
