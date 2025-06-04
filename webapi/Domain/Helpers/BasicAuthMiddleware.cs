using System.Text;

public class BasicAuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _username = "admin";
    private readonly string _password = "V0c4lc0m";

    public BasicAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string authHeader = context.Request.Headers["Authorization"];
        if (authHeader != null && authHeader.StartsWith("Basic "))
        {
            // Extract credentials
            var encodedUsernamePassword = authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();
            var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));
            var username = decodedUsernamePassword.Split(':', 2)[0];
            var password = decodedUsernamePassword.Split(':', 2)[1];

            // Check if the username and password are correct
            if (username == _username && password == _password)
            {
                await _next.Invoke(context);
                return;
            }
        }

        // Return authentication type (we will return a 401 if authentication fails)
        context.Response.Headers["WWW-Authenticate"] = "Basic";

        // Return unauthorized
        context.Response.StatusCode = 401;
    }
}
