using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ftreel.Constants;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Roles
{
    ROLE_USER,
    ROLE_ADMIN
}