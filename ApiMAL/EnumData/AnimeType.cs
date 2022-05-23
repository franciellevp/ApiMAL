using System.Text.Json.Serialization;

namespace ApiMAL.EnumData
{
    /// <summary>
    /// Enum to classify the possible types of each anime of the user list
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AnimeType
    {
        TV = 1,
        Movie,
        OVA,
        ONA
    }
}