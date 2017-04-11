using System.Runtime.Serialization;

namespace CapitalOne.CodingExcercise.SummaryApi.SourceModels
{
    /// <summary>
    /// Common arguments to call the endpoints.
    /// </summary>
    [DataContract(Name = "args")]
    public class CommonArgs
    {
        /// <summary>
        /// Required on almost every request.
        /// </summary>
        [DataMember(Name= "uid")]
        public long UId { get; set; }

        /// <summary>
        /// Token required on almost every request.
        /// </summary>
        [DataMember(Name = "token")]
        public string Token { get; set; }

        /// <summary>
        /// A per-application token used to prevent weirdos we don't know from using our API. Ask Gregor for a token if you don't have one.
        /// </summary>
        [DataMember(Name = "api-token")]
        public string ApiToken { get; set; }

        /// <summary>
        /// If true, returns an error if you're passing in JSON that doesn't respect the spec(e.g.fields that don't exist) not guaranteed to be SUPER strict, because this is actually a bit of a pain the butt to implement.
        /// </summary>
        [DataMember(Name = "json-strict-mode")]
        public bool JsonStrictMode { get; set; }

        /// <summary>
        /// If true, skip the strip-json step in encoding the response and send every field, but still consolidate error and error2 into a single error field.
        /// </summary>
        [DataMember(Name = "json-verbose-response")]
        public bool JsonVerboseResponse { get; set; }
    }
}
