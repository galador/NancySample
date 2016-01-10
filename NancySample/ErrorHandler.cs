using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Nancy;
using Nancy.ErrorHandling;
using Nancy.Extensions;
using Nancy.Responses;
using Nancy.ViewEngines;
using NancySample.Models;
using NancySample.Modules.Application;

namespace NancySample
{
    //Some of this code inspired by http://paulstovell.com/blog/consistent-error-handling-with-nancy
    public class ErrorHandler : IStatusCodeHandler
    {
        private readonly IViewRenderer _renderer;
        public List<HttpStatusCode> HandledCodes = new List<HttpStatusCode>
        {
            HttpStatusCode.NotFound,
            HttpStatusCode.InternalServerError,
            HttpStatusCode.Forbidden,
            HttpStatusCode.Unauthorized,
        };

        //Constructor
        public ErrorHandler(IViewRenderer viewRenderer)
        {
            _renderer = viewRenderer;
        }

        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            return HandledCodes.Contains(statusCode);
        }

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            var error = context.GetException();

            var resp = new ErrorResponse
            {
                Context = context,
                ShowDetails = Debugger.IsAttached
            };

            switch (statusCode)
            {
                case HttpStatusCode.Unauthorized:
                    context.Response = new RedirectResponse(RouteNames.GetLogin);
                    break;

                case HttpStatusCode.Forbidden:
                    resp.Title = "Permissions";
                    resp.Summary = "Sorry, you do not have permission to perform that action.";
                    break;

                case HttpStatusCode.NotFound:
                    resp.Title = "404 Not Found";
                    resp.Summary = "Sorry, the requested resource was not found.";
                    break;

                case HttpStatusCode.InternalServerError:
                    resp.Title = "Sorry, something went wrong";
                    resp.Summary = "An unexpected error occured.";
                    resp.Details = error.Message;
                    break;
            }
            context.Response.StatusCode = statusCode;
            context.Response = _renderer.RenderView(context, "Modules/Application/Views/Error.cshtml", resp);
        }
    }
}
