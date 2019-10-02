using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace ES.Infrastructure.Serialization
{
    // <summary>
    /// Json.Net serializer.
    /// </summary>
    /// 
    /// <seealso cref="http://www.newtonsoft.com/json" />
    public class JsonNetSerializer : IJsonSerializer
    {
        public string Serialize(object value, SerializerSettings settings = null)
        {
            return JsonConvert.SerializeObject(value, ReadSettings(settings));
        }

        public T Deserialize<T>(string json, SerializerSettings settings = null)
        {
            return JsonConvert.DeserializeObject<T>(json, ReadSettings(settings));
        }

        public dynamic Deserialize(string json)
        {
            return JToken.Parse(json);
        }

        /// <summary>
        /// Converts serializer settings to Json.Net serialization settings.
        /// </summary>
        /// 
        /// <param name="settings">Serializer settings.</param>
        /// 
        /// <returns>Json.Net serializer settings.</returns>
        private JsonSerializerSettings ReadSettings(SerializerSettings settings)
        {
            if (settings == null)
                return null;

            var jsonNetSettings = new JsonSerializerSettings
            {
                NullValueHandling = (bool)settings?.IgnoreNullValues ? NullValueHandling.Ignore : NullValueHandling.Include
            };

            if (settings?.ContractResolver != null)
                jsonNetSettings.ContractResolver = settings.ContractResolver as IContractResolver;

            return jsonNetSettings;
        }
    }
}
