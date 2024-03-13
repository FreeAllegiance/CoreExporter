using FreeAllegiance.IGCCore.Modules;
using FreeAllegiance.IGCCore.Util;
using System.Collections;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace CoreExporter
{
    internal class BitArrayToU64Converter : JsonConverter<System.Collections.BitArray>
    {
        public override BitArray? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, BitArray value, JsonSerializerOptions options)
        {
            var len = Math.Min(64, value.Count);
            ulong n = 0;
            for (int i = 0; i < len; i++)
            {
                if (value.Get(i))
                    n |= 1UL << i;
            }
            
            JsonValue v = JsonValue.Create(n);  
            v.WriteTo(writer);
        }
    }
}