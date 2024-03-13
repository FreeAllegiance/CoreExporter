using FreeAllegiance.IGCCore.Modules;
using FreeAllegiance.IGCCore.Util;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace CoreExporter
{
    internal class AGCObjectTypeConverter : JsonConverter<IGCCoreObject>
    {
        public override IGCCoreObject Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, IGCCoreObject value, JsonSerializerOptions options)
        {

            writer.WriteStartObject();
            writer.WriteString("_type", value.GetType().AssemblyQualifiedName);
            writer.WritePropertyName("_object");

            if (typeof(IGCCoreShip) == value.GetType())
            {
                JsonSerializer.Serialize(writer, (IGCCoreShip)value, typeof(IGCCoreShip), options);
                //SerializeAbilities(writer, (IGCCoreShip)value, options);
            }
            else if (typeof(IGCCoreDevel) == value.GetType())
                JsonSerializer.Serialize(writer, (IGCCoreDevel)value, typeof(IGCCoreDevel), options);
            else
                JsonSerializer.Serialize(writer, value, value.GetType(), options);

            writer.WriteEndObject();
        }

        

        private void SerializeAbilities(Utf8JsonWriter writer, IGCCoreShip value, JsonSerializerOptions options)
        {
            JsonArray array = new JsonArray();

            for (int i = 0; i < value.Abilities.Count; i++)
            {
                if(value.Abilities[i] == true)
                    array.Add( ((AbilityType) i).ToString());
            }

            writer.WritePropertyName("AbilityList");
            array.WriteTo(writer);
        }
    }
}