using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Text;
namespace webapi.Domain.Helpers
{
    public class BasicAuthFilter: TypeFilterAttribute
    {
        public BasicAuthFilter() : base(typeof(BasicAuthFilterImpl))
        {
        }

        private class BasicAuthFilterImpl : IAuthorizationFilter
        {
            public void OnAuthorization(AuthorizationFilterContext context)
            {
                string authHeader = context.HttpContext.Request.Headers["Authorization"];
                if (authHeader != null && authHeader.StartsWith("Basic"))
                {
                    // Extract credentials from header and validate (this is a very basic example)
                    var encodedUsernamePassword = authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();
                    var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));
                    var username = decodedUsernamePassword.Split(':', 2)[0];
                    var password = decodedUsernamePassword.Split(':', 2)[1];
                    if (username.Equals("admin", StringComparison.OrdinalIgnoreCase) && password.Equals("V0c4lc0m"))
                        return;
                }

                context.Result = new UnauthorizedResult();  // Return unauthorized if validation fails
            }
        }
    }
}