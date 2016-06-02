using System;

namespace Headmaster.Configuration
{
    public class HeaderVersioningOptions
    {
        public string MediaType { get; }
        public string MediaTypeParameter { get; }

        public HeaderVersioningOptions(string mediaType, string mediaTypeParameter)
        {
            if (string.IsNullOrEmpty(mediaType)) throw new ArgumentNullException(nameof(mediaType));
            if (string.IsNullOrEmpty(mediaTypeParameter)) throw new ArgumentNullException(nameof(mediaTypeParameter));
            MediaType = mediaType;
            MediaTypeParameter = mediaTypeParameter;
        }
    }
}