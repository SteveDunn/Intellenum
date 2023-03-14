using System;
using System.Linq;

namespace Intellenum;

internal static class EnumExtensions
{
    private static readonly int _maxConversion = Enum.GetValues(typeof(Conversions)).Cast<int>().Max() * 2;
    private static readonly int _maxCustomization = Enum.GetValues(typeof(Customizations)).Cast<int>().Max() * 2;

    public static bool IsValidFlags(this Conversions value) => (int) value >= 0 && (int) value < _maxConversion;
    
    public static bool IsValidFlags(this Customizations value) => (int) value >= 0 && (int) value < _maxCustomization;
    
}