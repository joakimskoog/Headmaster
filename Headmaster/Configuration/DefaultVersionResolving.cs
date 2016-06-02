namespace Headmaster.Configuration
{
    public enum DefaultVersionResolving
    {
        /// <summary>
        /// An error will be thrown if the supplied version parameter in the header is not present.
        /// </summary>
        ThrowErrorIfEmpty = 0,

        /// <summary>
        /// The latest version will be used if the supplied version parameter in the header is not present.
        /// </summary>
        UseLatestIfEmpty = 1
    }
}