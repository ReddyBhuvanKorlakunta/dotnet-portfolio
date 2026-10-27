namespace EnterprisePortal.Web.Middleware;
public class RequestIdMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext ctx)
    {
        var id = Guid.NewGuid().ToString("N")[..12];
        ctx.Items["RequestId"] = id;
        ctx.Response.Headers["X-Request-Id"] = id;
        await next(ctx);
    }
}
public class SecurityHeadersMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext ctx)
    {
        ctx.Response.Headers["X-Content-Type-Options"] = "nosniff";
        ctx.Response.Headers["X-Frame-Options"] = "DENY";
        ctx.Response.Headers["Referrer-Policy"] = "no-referrer";
        ctx.Response.Headers["Content-Security-Policy"] = "default-src 'self'; script-src 'self'; style-src 'self'";
        await next(ctx);
    }
}
