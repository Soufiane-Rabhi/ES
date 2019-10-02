namespace ES.Infrastructure.Serialization
{
    /// <summary>
    /// Json serializer settings.
    /// </summary>
    public class SerializerSettings
    {
        /// <summary>
        /// Gets or sets whether null values should be ignored during serialization / deserialization.
        /// </summary>
        public bool IgnoreNullValues { get; set; } = true;

        /// <summary>
        /// Gets or sets property names contract resolver.
        /// </summary>
        public object ContractResolver { get; set; }
    }
}
