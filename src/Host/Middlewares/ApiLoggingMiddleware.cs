using Core.Interfaces;
using Core.Entities;

namespace Host.Middlewares
{
    public class ApiLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IApiCallLogRepository _logRepository;

        public ApiLoggingMiddleware(RequestDelegate next, IApiCallLogRepository logRepository)
        {
            _next = next;
            _logRepository = logRepository;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = await FormatRequest(context.Request);
            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                var response = await FormatResponse(context.Response);
                await LogApiCall(context.Request.Path, request, response, context.Response.StatusCode);

                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            request.EnableBuffering();
            var body = request.Body;

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            var memoryBuffer = new Memory<byte>(buffer);
            await request.Body.ReadAsync(memoryBuffer);
            var bodyAsText = System.Text.Encoding.UTF8.GetString(buffer);
            request.Body = body;

            return $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}";
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return $"Response {text}";
        }

        private async Task LogApiCall(string path, string request, string response, int statusCode)
        {
            var log = new ApiCallLog
            {
                Endpoint = path,
                Request = request,
                Response = response,
                Timestamp = DateTime.UtcNow,
                Success = statusCode >= 200 && statusCode < 400
            };

            await _logRepository.AddAsync(log);
        }
    }
}
