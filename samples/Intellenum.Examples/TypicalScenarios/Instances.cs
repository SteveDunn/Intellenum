using System;
using System.Threading.Tasks;
// ReSharper disable RedundantCast

namespace Intellenum.Examples.TypicalScenarios.Instances
{
    internal class InstanceExamples : IScenario
    {
        public Task Run()
        {
            VendorInformation vi = new VendorInformation();
            Console.WriteLine((bool) (vi.VendorId == VendorType.Unspecified)); // true
            Console.WriteLine((bool) (vi.VendorId != VendorType.Invalid)); // true

            // from a text file that is screwed, we'll end up with:
            var invalidVi = VendorInformation.FromTextFile();

            Console.WriteLine((bool) (invalidVi.VendorId == VendorType.Invalid)); // true
            
            return Task.CompletedTask;
        }
    }

    [Intellenum]
    public partial class ImpliedFieldName
    {
        public static readonly ImpliedFieldName Instance1 = new(1);
    }


    [Intellenum<float>]
    [Instance("Freezing", 0.0f)]
    [Instance("Boiling", 100.0f)]
    [Instance("AbsoluteZero", -273.15f)]
    public partial class Centigrade
    {
    }

    [Intellenum<string>]
    [Instance("Unspecified", "[UNSPCFD]")]
    [Instance("Invalid", "[INVLD]")]
    [Instance("Standard", "[STD]")]
    [Instance("Preferred", "[PRF]")]
    public partial class VendorType
    {
    }
    
    [Intellenum]
    [Instance("Salt", 1)]
    [Instance("Pepper", 2)]
    public partial class CondimentMixedInstances
    {
        public static readonly CondimentMixedInstances Mayo = new CondimentMixedInstances("Mayo", 5);
        public static readonly CondimentMixedInstances Ketchup = new CondimentMixedInstances("Ketchup", 6);

        static CondimentMixedInstances()
        {
            Instance("Vinegar", 3);
            Instance("Mustard", 4);
        }
    }    

    public class VendorInformation
    {
        public VendorType VendorId { get; private init; } = VendorType.Standard;

        public static VendorInformation FromTextFile()
        {
            // imagine the text file is screwed...
            return new VendorInformation
            {
                VendorId = VendorType.Invalid
            };
        }
    }
}