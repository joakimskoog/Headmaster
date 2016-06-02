using System;
using System.Linq;
using System.Net.Http;

namespace Headmaster.Core
{
    public static class HttpRequestMessageExtensions
    {
        public static string GetApiVersion(this HttpRequestMessage request, string mediaType, string mediaTypeParameter)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrEmpty(mediaType)) throw new ArgumentNullException(nameof(mediaType));
            if (string.IsNullOrEmpty(mediaTypeParameter)) throw new ArgumentNullException(nameof(mediaTypeParameter));

            var acceptHeader = request.Headers.Accept;

            foreach (var mime in acceptHeader.OrderByDescending(x => x.Quality))
            {
                if (string.Equals(mime.MediaType, mediaType, StringComparison.OrdinalIgnoreCase))
                {
                    var version = mime.Parameters.FirstOrDefault(x => string.Equals(x.Name, mediaTypeParameter, StringComparison.OrdinalIgnoreCase));

                    return version?.Value ?? string.Empty;
                }
            }

            return string.Empty;
        }
    }
}