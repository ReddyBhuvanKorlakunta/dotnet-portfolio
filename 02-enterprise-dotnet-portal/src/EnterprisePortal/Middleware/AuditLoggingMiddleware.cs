namespace EnterprisePortal.Middleware;

// Audit: who + what + when + request ID. Never log the sensitive value itself.
public class AuditLoggingMiddleware(RequestDelegate next, ILogger<AuditLoggingMiddleware> log)
{
    public async Task InvokeAsync(HttpContext ctx)
    {
        var requestId = Guid.NewGuid().ToString("N")[..12];
        ctx.Items["RequestId"] = requestId;
        await next(ctx);
        log.LogInformation("Request {Id} {Method} {Path} -> {Status} user={User}",
            requestId, ctx.Request.Method, ctx.Request.Path, ctx.Response.StatusCode,
            ctx.User.Identity?.Name ?? "anonymous");
    }
}
public class SecurityHeadersMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext ctx)
    {
        ctx.Response.Headers["X-Content-Type-Options"] = "nosniff";
        ctx.Response.Headers["X-Frame-Options"] = "DENY";
        ctx.Response.Headers["Content-Security-Policy"] = "default-src 'self'";
        await next(ctx);
    }
}
