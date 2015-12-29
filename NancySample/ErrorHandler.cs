using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Nancy;
using Nancy.ErrorHandling;
using Nancy.ViewEngines;

namespace NancySample
{
    public class ErrorHandler : IStatusCodeHandler
    {
        //Stores all the available status codes for the lifetime of the 
        //request. If we try to load a view for this status code, and it 
        //doesn't exist, that code will be removed from this list, so we
        //don't try to handle it if it happens again in future requests.
        public static List<int> AvailableStatusCodes { get; set; }
        private readonly IViewRenderer _renderer;

        public ErrorHandler(IViewRenderer viewRenderer)
        {
            _renderer = viewRenderer;

            if (Debugger.IsAttached)
            {
                AvailableStatusCodes = new List<int>();
            }
            else
            {
                //Initially, add all HTTP Error codes (except OK & See Other)
                AvailableStatusCodes = (
                    from HttpStatusCode x in Enum.GetValues(typeof(HttpStatusCode))
                    where !new[] { HttpStatusCode.OK, HttpStatusCode.SeeOther }.Contains(x)
                    select (int)x
                ).ToList();
            }
        }


        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            return AvailableStatusCodes.Any(x => x == (int) statusCode);
        }

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            try
            {
                context.Response = _renderer.RenderView(context, "/Errors/" + (int) statusCode + ".cshtml");
                context.Response.StatusCode = statusCode;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.ToString());
                AvailableStatusCodes.Remove((int) statusCode);
                context.Response.StatusCode = statusCode;
            }
        }
    }
}
