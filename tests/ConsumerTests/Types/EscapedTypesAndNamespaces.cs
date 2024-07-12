#pragma warning disable IDE1006 // Naming Styles
using record.@struct.@float;

namespace record.@struct.@float
{
    public readonly record struct @decimal() : IComparable<@decimal>
    {
        public int CompareTo(@decimal other) => throw new NotImplementedException();
    }
    
}

namespace @double
{
    public readonly record struct @decimal() : IComparable<@decimal>
    {
        private static int n;

        public int Number { get; } = ++n;
        
        public int CompareTo(@decimal other) => throw new NotImplementedException();
    }

    [Intellenum(typeof(@decimal))]
#pragma warning disable INTELLENUM030
    public partial class classFromEscapedNamespaceWithReservedUnderlyingType
#pragma warning restore INTELLENUM030
    {
        static classFromEscapedNamespaceWithReservedUnderlyingType()
        {
            Member("One", new @decimal());
            Member("Two", new @decimal());
        }
    }

    [Intellenum]
    [Member("Item1", 1)]
    [Member("Item2", 2)]
    public partial class classFromEscapedNamespace
    {
    }
}

namespace @bool.@byte.@short.@float.@object
{
    [Intellenum]
    [Member("Item1", 1)]
    [Member("Item2", 2)]
    public partial class @class
    {
    }

    [Intellenum]
    [Member("Item1", 1)]
    [Member("Item2", 2)]
    public partial class @event
    {
    }

    [Intellenum(typeof(record.@struct.@float.@decimal))]
    public partial class @event2
    {
        static event2()
        {
            Member("Item1", new @decimal());
            Member("Item2", new @decimal());

        }
    }
}