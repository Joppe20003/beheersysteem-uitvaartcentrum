using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using System.Text.Encodings.Web;

namespace beheersysteem_uitvaartcentrum.backend.api.Filters
{
    public class SanitizeInputFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument == null) continue;

                SanitizeObject(argument);
            }
        }

        private void SanitizeObject(object obj)
        {
            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.PropertyType == typeof(string) && p.CanWrite);

            foreach (var prop in properties)
            {
                var value = prop.GetValue(obj) as string;
                if (value != null)
                {
                    prop.SetValue(obj, HtmlEncoder.Default.Encode(value));
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
