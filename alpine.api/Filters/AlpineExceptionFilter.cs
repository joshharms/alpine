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

            var error = new AlpineCreateResponse().Error( ( int )HttpStatusCode.BadRequest, "An error has occured.", message, false );

            if ( exception is AlpineException )
            {
                error = new AlpineCreateResponse().Error( ( int )HttpStatusCode.BadRequest, exception.Message, "", ( exception is AlpineException ) );
            }

            if ( exception is UnauthorizedAccessException )
            {
                error.Meta.Code = ( int )HttpStatusCode.Unauthorized;
                context.HttpContext.Response.StatusCode = ( int )HttpStatusCode.Unauthorized;
            }
            else
            {
                context.HttpContext.Response.StatusCode = ( int )HttpStatusCode.BadRequest;
            }


            context.Result = new JsonResult( error );

            base.OnException( context );
        }
    }
}
