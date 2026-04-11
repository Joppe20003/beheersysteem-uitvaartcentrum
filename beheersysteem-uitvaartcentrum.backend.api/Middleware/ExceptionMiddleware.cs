using beheersysteem_uitvaartcentrum.backend.application.Exceptions;

namespace beheersysteem_uitvaartcentrum.backend.api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        } 

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundForeignKey notFoundForeignKey)
            {
                context.Response.StatusCode = 410;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new
                {
                    type = "https://tools.ietf.org/html/rfc9110#section-15.5.5",
                    title = notFoundForeignKey.Title,
                    status = 410,
                    errors = new { message = new[] { notFoundForeignKey.Message } },
                    trace = context.TraceIdentifier
                });
            }
            catch (NotAllowedFileExtension notAllowedFileExtension)
            {
                context.Response.StatusCode = 415;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new
                {
                    type = "https://tools.ietf.org/html/rfc9110#section-15.5.16",
                    title = notAllowedFileExtension.Title,
                    status = 415,
                    errors = new { message = new[] { notAllowedFileExtension.Message } },
                    trace = context.TraceIdentifier
                });
            }
            catch (Exception exception)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new
                {
                    type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                    title = "Er is een fout opgetreden",
                    status = 500,
                    errors = new { message = new[] { exception.Message } },
                    trace = context.TraceIdentifier
                });
            }
        }
    }
}
