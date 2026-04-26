using Yarp.ReverseProxy.Forwarder;

namespace MyApi.Bootstrap
{
    public sealed class PlayFabForwardingTransformer : HttpTransformer
    {
        public override async ValueTask TransformRequestAsync(
            HttpContext httpContext,
            HttpRequestMessage proxyRequest,
            string destinationPrefix,
            CancellationToken cancellationToken)
        {
            await base.TransformRequestAsync(
                httpContext, proxyRequest, destinationPrefix, cancellationToken);

            var clientIp = HelperClass.GetClientIp(httpContext);

            proxyRequest.Headers.Remove("X-Forwarded-For");
            proxyRequest.Headers.TryAddWithoutValidation("X-Forwarded-For", clientIp);
            proxyRequest.Headers.Remove("X-Real-IP");
            proxyRequest.Headers.TryAddWithoutValidation("X-Real-IP", clientIp);
        }
    }
}