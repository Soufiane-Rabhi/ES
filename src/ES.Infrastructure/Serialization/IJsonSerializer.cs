namespace ES.Infrastructure.Serialization
{
    /// <summary>
    /// Manages Json serialization / deserialization.
    /// </summary>
    public interface IJsonSerializer
    {
        /// <summary>
        /// Serializes an object to json representation string.
        /// </summary>
        /// 
        /// <param name="value">Object to serialize.</param>
        /// <param name="settings">Serializer settings.</param>
        /// 
        /// <returns>Json string.</returns>
        string Serialize(object value, SerializerSettings settings = null);

        /// <summary>
        /// Deserializes an json string to a strongly typed object.
        /// </summary>
        /// 
        /// <typeparam name="T">Target object type.</typeparam>
        /// 
        /// <param name="json">Json to deserialize.</param>
        /// <param name="settings">Serializer settings.</param>
        /// 
        /// <returns>Deserialized object.</returns>
        T Deserialize<T>(string json, SerializerSettings settings = null);

        /// <summary>
        /// Deserializes an json string to a dynamic object.
        /// </summary>
        /// 
        /// <param name="json">Json to deserialize.</param>
        /// 
        /// <returns>Deserialized object.</returns>
        dynamic Deserialize(string json);
    }
}
