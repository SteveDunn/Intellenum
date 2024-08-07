﻿using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

// ReSharper disable RedundantCast

namespace Intellenum.Examples.TypicalScenarios.Members;

[UsedImplicitly]
internal class MembersScenario : IScenario
{
    public Task Run()
    {
        VendorInformation vi = new VendorInformation();
        Console.WriteLine(vi.VendorType == VendorType.Unspecified); // true
        Console.WriteLine(vi.VendorType != VendorType.Invalid); // true

        // from a text file that is screwed, we'll end up with:
        var invalidVi = VendorInformation.FromTextFile();

        Console.WriteLine((bool) (invalidVi.VendorType == VendorType.Invalid)); // true
        
        Console.WriteLine("Implied field, \"Member1\" has a value of " + ImpliedMemberNames.Member1); // Implied field, "Member1" is 1
            
        return Task.CompletedTask;
    }
}

[Intellenum<string>]
[Members("Zero, One, Two, Three")]
[Member("Four")]
[Member("Five")]
[Member("Six")]
public partial class Mixture
{
    static Mixture()
    {
        Member("Nine");
        Member("Ten");
        Members("Eleven, Twelve");
        Member("Thirteen");
    }
    public static readonly Mixture Seven = new();
    public static readonly Mixture Eight = new();
}

[Intellenum]
public partial class ImpliedMemberNames
{
    public static readonly ImpliedMemberNames Member1 = new(1);
    public static readonly ImpliedMemberNames Member2 = new(2);
}


[Intellenum<float>]
[Member("Freezing", 0.0f)]
[Member("Boiling", 100.0f)]
[Member("AbsoluteZero", -273.15f)]
public partial class Centigrade
{
}

[Intellenum<string>]
[Member("Unspecified", "[UNSPCFD]")]
[Member("Invalid", "[INVLD]")]
[Member("Standard", "[STD]")]
[Member("Preferred", "[PRF]")]
public partial class VendorType
{
}
    
[Intellenum]
[Member("Salt", 1)]
[Member("Pepper", 2)]
public partial class CondimentMixedMembers
{
    public static readonly CondimentMixedMembers Mayo = new CondimentMixedMembers("Mayo", 5);
    public static readonly CondimentMixedMembers Ketchup = new CondimentMixedMembers("Ketchup", 6);

    static CondimentMixedMembers()
    {
        Member("Vinegar", 3);
        Member("Mustard", 4);
    }
}    

public class VendorInformation
{
    public VendorType VendorType { get; private init; } = VendorType.Standard;

    public static VendorInformation FromTextFile()
    {
        // imagine the text file is screwed...
        return new VendorInformation
        {
            VendorType = VendorType.Invalid
        };
    }
}