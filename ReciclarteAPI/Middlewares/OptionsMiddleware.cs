using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ReciclarteAPI.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class OptionsMiddleware
    {
        private readonly RequestDelegate _next;

        public OptionsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {

            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, X-CSRF-Token, X-Requested-With, Accept, Accept-Version, Accept-Encoding, Content-Length, Content-MD5, Date, X-Api-Version, X-File-Name");
            context.Response.Headers.Add("Access-Control-Allow-Methods", "POST,GET,PUT,PATCH,DELETE,OPTIONS");
            if (context.Request.Method == "OPTIONS")
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                return;
                //await context.Response.WriteAsync("Hola");
            }
            await _next(context);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class OptionsMiddlewareExtensions
    {
        public static IApplicationBuilder UseOptionsMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<OptionsMiddleware>();
        }
    }
}
