using System;

namespace Intellenum;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
public class MembersAttribute : Attribute
{
    /// <summary>
    /// Represents an attribute that is used to define the names of members.
    /// <param name="names">A comma separated list of names.</param>
    /// </summary>
    public MembersAttribute(string names) { }
}