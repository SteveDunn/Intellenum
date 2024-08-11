using System.Threading;

namespace Intellenum;

/// <summary>
/// A counter used to get the next value for int enums
/// that don't specify a value themselves.
/// </summary>
internal class Counter
{
    private int _value = -1;

    public Counter Increment()
    {
        Interlocked.Increment(ref _value);
        return this;
    }

    public int Value => _value;
    public string ValueAsText => _value.ToString();
}