using FreeAllegiance.IGCCore;
using FreeAllegiance.IGCCore.Modules;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace CoreExporter
{
    internal class Program
    {
        private enum GlobalDefinitions
        {
            AllowExpansion = 5,
            AllowShipyard = 6,
            AllowSupremacy = 3,
            AllowTacticalLab = 4
        }

        static void Main(string[] args)
        {
            var core = new Core(@"C:\Source\git\Artwork\ac_13d.igc");

            //var modifier = new IgnorePropertiesWithName("Defs", "Pres", "TechTreeLocal");
            var partMaskModifier = new BitArrayModifier("PartMask", new BitArrayConverter<PartMaskType>());
            var abilityModifier = new BitArrayModifier("Abilities", new BitArrayConverter<AbilityType>());
            var defsModifier = new BitArrayModifier("Defs", new BitArrayToNumberConverter());
            var presModifier = new BitArrayModifier("Pres", new BitArrayToNumberConverter());
            var techTreeLocalModifier = new BitArrayModifier("TechTreeLocal", new BitArrayToNumberConverter());
            var stationAbilityModifier = new BitArrayModifier("StationAbility", new BitArrayConverter<StationAbilityType>());
            var featuresModifier = new BitArrayModifier("Features", new BitArrayConverter<ExpendableAbilityType>());
            var useMaskModifier = new BitArrayModifier("UseMask", new BitArrayToU64Converter());

            var useMaskListModifier = new BitArrayModifier("UseMasks", new BitArrayListToU64Converter());


            JsonSerializerOptions options = new();
            options.IncludeFields = true;
            options.TypeInfoResolver = new DefaultJsonTypeInfoResolver
            {
                Modifiers =
                    {
                        //modifier.ModifyTypeInfo,
                        partMaskModifier.ModifyTypeInfo,
                        abilityModifier.ModifyTypeInfo,
                        defsModifier.ModifyTypeInfo,
                        presModifier.ModifyTypeInfo,
                        techTreeLocalModifier.ModifyTypeInfo,
                        stationAbilityModifier.ModifyTypeInfo,
                        featuresModifier.ModifyTypeInfo,
                        useMaskModifier.ModifyTypeInfo,
                        useMaskListModifier.ModifyTypeInfo
                    }
            };

            options.Converters.Add(new AGCObjectTypeConverter());
            options.WriteIndented = true;

            var s = JsonSerializer.Serialize(core, options);

            File.WriteAllText(@"c:\1\2\ac_13d.json", s);


            //TechTree tree = new TechTree();
            //tree.LoadCoreFile(@"C:\Source\git\Artwork\ac_13d.igc");

            //var modifier = new IgnorePropertiesWithName("Defs", "Pres", "TechTreeLocal");
            //var partMaskModifier = new BitArrayModifier("PartMask", new BitArrayConverter<PartMaskType>());
            //var abilityModifier = new BitArrayModifier("Abilities", new BitArrayConverter<AbilityType>());

            //foreach (var faction in tree.FactionNodes)
            //{
            //    tree.BuildFactionNodeTreeFromCore(faction);

            //   JsonSerializerOptions options = new();
            //    options.IncludeFields = true;
            //    options.TypeInfoResolver = new DefaultJsonTypeInfoResolver
            //    {
            //        Modifiers = 
            //        { 
            //            modifier.ModifyTypeInfo,
            //            partMaskModifier.ModifyTypeInfo,
            //            abilityModifier.ModifyTypeInfo
            //        }
            //    };

            //    options.Converters.Add(new AGCObjectTypeConverter());
            //    options.WriteIndented = true;

            //var s = JsonSerializer.Serialize(tree.RootNode, options);

            //File.WriteAllText(@"c:\1\2\" + faction.NodeName + ".json", s);
        //}

        }

        
    }
}
