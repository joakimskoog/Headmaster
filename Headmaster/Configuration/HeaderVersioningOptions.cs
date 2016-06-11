using System;

namespace Headmaster.Configuration
{
    public class HeaderVersioningOptions
    {
        public Func<string,string> RequestVersionTransformer { get; }
        public string MediaType { get; }
        public string MediaTypeParameter { get; }
        public DefaultVersionResolving DefaultVersionResolving { get; set; }

        public HeaderVersioningOptions(string mediaType, string mediaTypeParameter, DefaultVersionResolving defaultVersionResolving, Func<string,string> requestVersionTransformer = null)
        {
            if (string.IsNullOrEmpty(mediaType)) throw new ArgumentNullException(nameof(mediaType));
            if (string.IsNullOrEmpty(mediaTypeParameter)) throw new ArgumentNullException(nameof(mediaTypeParameter));
            MediaType = mediaType;
            MediaTypeParameter = mediaTypeParameter;
            DefaultVersionResolving = defaultVersionResolving;
            RequestVersionTransformer = requestVersionTransformer ?? (requestVersion => requestVersion);
        }
    }
}