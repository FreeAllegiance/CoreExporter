using FreeAllegiance.IGCCore.Modules;
using FreeAllegiance.IGCCore.Util;
using System.Collections;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace CoreExporter
{
    internal class BitArrayListToU64Converter : JsonConverter<BitArray[]>
    {
        public override BitArray[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, BitArray[] value, JsonSerializerOptions options)
        {
            JsonArray array = new JsonArray();

            int useMaskCounter = 0; 
            foreach (var bitArray in value)
            {
                var len = Math.Min(64, bitArray.Count);
                ulong n = 0;
                for (int i = 0; i < len; i++)
                {
                    if (bitArray.Get(i))
                        n |= 1UL << i;
                }

                

                array.Add(JsonNode.Parse($"{{\"{((UseMaskType) useMaskCounter).ToString()}\": {n}}}"));
                useMaskCounter++;
            }

            array.WriteTo(writer);
        }
    }
}