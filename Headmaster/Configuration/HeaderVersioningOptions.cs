using System;

namespace Headmaster.Configuration
{
    public class HeaderVersioningOptions
    {
        public string MediaType { get; }
        public string MediaTypeParameter { get; }
        public string RequestVersionPrepend { get; set; }
        public DefaultVersionResolving DefaultVersionResolving { get; set; }

        public HeaderVersioningOptions(string mediaType, string mediaTypeParameter, DefaultVersionResolving defaultVersionResolving, string requestVersionPrepend = "")
        {
            if (string.IsNullOrEmpty(mediaType)) throw new ArgumentNullException(nameof(mediaType));
            if (string.IsNullOrEmpty(mediaTypeParameter)) throw new ArgumentNullException(nameof(mediaTypeParameter));
            if(requestVersionPrepend == null) throw new ArgumentNullException(nameof(requestVersionPrepend));
            MediaType = mediaType;
            MediaTypeParameter = mediaTypeParameter;
            DefaultVersionResolving = defaultVersionResolving;
            RequestVersionPrepend = requestVersionPrepend;
        }
    }
}