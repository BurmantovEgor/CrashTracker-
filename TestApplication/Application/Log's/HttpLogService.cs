using System.Text;

namespace TestApplication.Application.Middleware
{
    public class HttpLogService
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpLogService> _logger;
        private readonly string _logFilePath;

        public HttpLogService(RequestDelegate next, ILogger<HttpLogService> logger)
        {
            _next = next;
            _logger = logger;
            _logFilePath = $"Log\\log_{DateTime.UtcNow:yyyy-MM-dd}.txt";
        }

        public async Task Invoke(HttpContext context)
        {
            var requestTime = DateTime.UtcNow;
            var ip = context.Connection.RemoteIpAddress?.ToString();
            var requestMethod = context.Request.Method;
            var requestPath = context.Request.Path;
            var queryString = context.Request.QueryString.HasValue ? context.Request.QueryString.ToString() : "No query parameters";

            context.Request.EnableBuffering();
            string requestBody = "Body is empty";
            if (context.Request.ContentLength > 0)
            {
                using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
                requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            var originalResponseBodyStream = context.Response.Body;
            using var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            await _next(context);

            var responseTime = DateTime.UtcNow;
            var statusCode = context.Response.StatusCode;

            var logEntry = new StringBuilder()
                .AppendLine("HTTP-Log")
                .AppendLine($"[{requestTime:HH:mm:ss}] | IP: {ip} | {requestMethod} {requestPath} -> {statusCode}")
                .AppendLine($"Query: {queryString.Trim()}")
                .AppendLine($"Request Body: {requestBody.Trim()}")
                .AppendLine($"Time Taken: {(responseTime - requestTime).TotalMilliseconds} ms");

            if (statusCode >= 400)
            {
                responseBodyStream.Seek(0, SeekOrigin.Begin);
                string responseBody = await new StreamReader(responseBodyStream, Encoding.UTF8).ReadToEndAsync();
                logEntry.AppendLine($"Response Body: {responseBody}");
                _logger.LogError(logEntry.ToString());
            }
            else
            {
                _logger.LogInformation(logEntry.ToString());
            }

            await File.AppendAllTextAsync(_logFilePath, logEntry.ToString() + Environment.NewLine);

            responseBodyStream.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(originalResponseBodyStream);
        }
    }
}
