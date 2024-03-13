using FreeAllegiance.IGCCore.Modules;
using FreeAllegiance.IGCCore.Util;
using System.Collections;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace CoreExporter
{
    internal class BitArrayConverter<T> : JsonConverter<System.Collections.BitArray> where T : System.Enum
    {
        public override BitArray? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        

        public override void Write(Utf8JsonWriter writer, BitArray value, JsonSerializerOptions options)
        {
            //writer.WriteStartObject();
            //writer.WriteString("_type", value.GetType().AssemblyQualifiedName);

            JsonArray array = new JsonArray();

            for (int i = 0; i < value.Count; i++)
            {
                if (value[i] == true)
                {
                    array.Add(Enum.GetName(typeof(T), i));
                }
            }

            //writer.WritePropertyName("PartType");
            array.WriteTo(writer);

            //writer.WriteEndObject();
        }
    }
}