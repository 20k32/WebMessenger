using System.Text.Json.Serialization;

namespace SharpMessenger.Domain.UiModels
{
    [JsonDerivedType(typeof(Data), typeDiscriminator: "base")]
    [JsonDerivedType(typeof(ComplexData), typeDiscriminator: "complex")]
    public class Data
    {
        public string UserName { get; set; } = null!;

        // for serialization purpose only
        public Data()
        { }
    }
}
