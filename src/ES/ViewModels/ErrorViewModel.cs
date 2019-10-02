namespace ES
{
    /// <summary>
    /// Error view model.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Gets or sets error status code.
        /// </summary>
        public int? Code { get; set; }

        /// <summary>
        /// Gets or sets error request id.
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Gets whether to show the request id or not.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
