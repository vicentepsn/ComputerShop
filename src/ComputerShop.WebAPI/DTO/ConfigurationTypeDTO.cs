using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace ComputerShop.WebAPI.DTO
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ConfigurationTypeDTO
    {
        [EnumMember(Value = "Ram")]
        Ram,
        [EnumMember(Value = "HDD")]
        HDD,
        [EnumMember(Value = "Colour")]
        Colour,
    }
}
