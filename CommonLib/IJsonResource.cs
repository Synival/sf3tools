using Newtonsoft.Json.Linq;

namespace CommonLib {
    public interface IJsonResource {
        /// <summary>
        /// Assigns members of this object from a JSON string.
        /// </summary>
        /// <param name="json">A JSON string of the object.</param>
        bool AssignFromJSON_String(string json);

        /// <summary>
        /// Assigns members of this object from a JToken.
        /// </summary>
        /// <param name="jToken">A JToken of the object.</param>
        bool AssignFromJToken(JToken jToken);

        /// <summary>
        /// Converts the object to a JSON string.
        /// </summary>
        /// <returns>A JSON string of the object.</returns>
        string ToJSON_String();

        /// <summary>
        /// Converts the object to a JToken.
        /// </summary>
        /// <returns>A JToken of the object.</returns>
        JToken ToJToken();
    }
}
