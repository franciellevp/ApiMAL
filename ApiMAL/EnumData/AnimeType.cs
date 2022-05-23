using System.Text.Json.Serialization;

namespace ApiMAL.EnumData
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AnimeType
    {
        TV = 1,
        Movie,
        OVA,
        ONA
    }
}