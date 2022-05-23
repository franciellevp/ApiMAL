using System.Text.Json.Serialization;

namespace ApiMAL.EnumData
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Status {
        Watching = 1,
        Completed,
        Onhold,
        Dropped,
        PlanToWatch
    }
}