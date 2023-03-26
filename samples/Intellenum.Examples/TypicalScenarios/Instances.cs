using System;
using System.Threading.Tasks;
// ReSharper disable RedundantCast

namespace Intellenum.Examples.TypicalScenarios.Instances
{

    /*
     * Instances allow us to create specific static readonly instances of this type.
     */

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

    public class VendorInformation
    {
        public VendorType VendorId { get; private init; } = VendorType.Standard;

        public static VendorInformation FromTextFile()
        {
            // imaging the text file is screwed...
            return new VendorInformation
            {
                VendorId = VendorType.Invalid
            };
        }
    }
}