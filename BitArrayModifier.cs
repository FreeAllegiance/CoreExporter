using System.Runtime;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace CoreExporter
{
    internal class BitArrayModifier
    {
        private readonly string _targetName;
        private readonly JsonConverter _converter;

        public BitArrayModifier(string targetName, JsonConverter converter)
        {
            _targetName = targetName;
            _converter = converter;
        }

        public void ModifyTypeInfo(JsonTypeInfo ti)
        {
            if (ti.Kind != JsonTypeInfoKind.Object)
                return;

            foreach (var prop in ti.Properties)
            {
                if (prop.Name == _targetName)
                {
                    prop.CustomConverter = _converter;
                }
            }
        }
    }   
}
