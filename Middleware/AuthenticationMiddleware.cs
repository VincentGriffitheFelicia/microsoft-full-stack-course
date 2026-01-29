namespace Course_Repository.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthenticationMiddleware> _logger;
        private static readonly HashSet<string> _publicRoutes = new()
        {
            "/",
            "/swagger",
            "/swagger/index.html",
            "/api/docs"
        };

        public AuthenticationMiddleware(RequestDelegate next, ILogger<AuthenticationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string path = context.Request.Path.Value?.ToLower() ?? "";

            // Skip authentication for public routes
            if (IsPublicRoute(path))
            {
                _logger.LogInformation($"Public route accessed: {path}");
                await _next(context);
                return;
            }

            // Check for authorization header
            if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                _logger.LogWarning($"Missing authorization header for {context.Request.Method} {path}");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new { error = "Missing authorization header" });
                return;
            }

            // Validate token format (Bearer scheme)
            var token = authHeader.ToString();
            if (!token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning($"Invalid authorization scheme for {context.Request.Method} {path}");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new { error = "Invalid authorization scheme. Use 'Bearer' scheme" });
                return;
            }

            // Extract and validate token
            var tokenValue = token.Substring("Bearer ".Length).Trim();
            if (!IsValidToken(tokenValue))
            {
                _logger.LogWarning($"Invalid token provided for {context.Request.Method} {path}");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new { error = "Invalid or expired token" });
                return;
            }

            _logger.LogInformation($"Valid token authenticated for {context.Request.Method} {path}");
            await _next(context);
        }

        private bool IsPublicRoute(string path)
        {
            // Check for exact matches or starts with for directories
            return path == "/" || 
                   path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase) ||
                   path.StartsWith("/api/docs", StringComparison.OrdinalIgnoreCase);
        }

        private bool IsValidToken(string token)
        {
            // Simple token validation: check if token matches pattern and is not empty
            // In production, validate JWT signature, expiration, etc.
            if (string.IsNullOrWhiteSpace(token))
                return false;

            // Accept tokens that are at least 20 characters (simple validation)
            // In production, use JWT validation library
            return token.Length >= 20 && !token.Contains(" ");
        }
    }
}
