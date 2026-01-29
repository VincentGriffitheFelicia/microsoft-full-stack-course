using System.Diagnostics;

namespace Course_Repository.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var originalBodyStream = context.Response.Body;

            // Log incoming request
            _logger.LogInformation(
                "Incoming Request: {Method} {Path} | Timestamp: {Timestamp}",
                context.Request.Method,
                context.Request.Path,
                DateTime.UtcNow);

            // Capture response
            using (var memoryStream = new MemoryStream())
            {
                context.Response.Body = memoryStream;

                try
                {
                    await _next(context);
                }
                finally
                {
                    stopwatch.Stop();

                    // Log outgoing response
                    _logger.LogInformation(
                        "Outgoing Response: {Method} {Path} | Status: {StatusCode} | Duration: {ElapsedMilliseconds}ms",
                        context.Request.Method,
                        context.Request.Path,
                        context.Response.StatusCode,
                        stopwatch.ElapsedMilliseconds);

                    // Rewind memory stream to beginning before copying
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    // Optionally read response body for richer logs (avoid large bodies in production)
                    try
                    {
                        using var reader = new StreamReader(memoryStream, leaveOpen: true);
                        var responseBody = await reader.ReadToEndAsync();
                        if (!string.IsNullOrEmpty(responseBody))
                        {
                            _logger.LogDebug("Response Body: {ResponseBody}", responseBody);
                        }
                        // Rewind again before copying
                        memoryStream.Seek(0, SeekOrigin.Begin);
                    }
                    catch
                    {
                        // ignore read errors for logging
                        memoryStream.Seek(0, SeekOrigin.Begin);
                    }

                    // Copy response back to original stream and restore
                    await memoryStream.CopyToAsync(originalBodyStream);
                    context.Response.Body = originalBodyStream;
                }
            }
        }
    }
}
