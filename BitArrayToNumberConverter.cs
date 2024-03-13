using FreeAllegiance.IGCCore.Modules;
using FreeAllegiance.IGCCore.Util;
using System.Collections;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace CoreExporter
{
    internal class BitArrayToNumberConverter : JsonConverter<System.Collections.BitArray>
    {
        public override BitArray? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, BitArray value, JsonSerializerOptions options)
        {
            JsonArray array = new JsonArray();

            for (int i = 0; i < value.Count; i++)
            {
                if (value[i] == true)
                {
                    array.Add(i);
                }
            }

            array.WriteTo(writer);
        }
    }
}