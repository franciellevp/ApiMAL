using System.Text.Json.Serialization;

namespace ApiMAL.EnumData
{
    /// <summary>
    /// Enum to classify the current Status of each anime of the user list
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Status {
        Watching = 1,
        Completed,
        Onhold,
        Dropped,
        PlanToWatch
    }
}