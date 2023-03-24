using FluentAssertions;
using Microsoft.CodeAnalysis;
using Xunit;

namespace Intellenum.Tests
{
    public class IntellenumConfigurationTests
    {
        public class DebuggerAttributeGenerationFlag
        {
            [Fact]
            public void Local_beats_global_when_specified()
            {
                var result = IntellenumConfiguration.Combine(
                    ConfigWithOmitDebugAs(DebuggerAttributeGeneration.Basic),
                    ConfigWithOmitDebugAs(DebuggerAttributeGeneration.Full),
                    () => new DummyNamedTypeSymbol());

                result.DebuggerAttributes.Should().Be(DebuggerAttributeGeneration.Basic);
            }

            [Fact]
            public void Uses_global_when_local_not_specified()
            {
                var result = IntellenumConfiguration.Combine(
                    ConfigWithOmitDebugAs(DebuggerAttributeGeneration.Default),
                    ConfigWithOmitDebugAs(DebuggerAttributeGeneration.Basic),
                    () => new DummyNamedTypeSymbol());

                result.DebuggerAttributes.Should().Be(DebuggerAttributeGeneration.Basic);
            }

            private static IntellenumConfiguration ConfigWithOmitDebugAs(DebuggerAttributeGeneration debuggerAttributes) =>
                new(
                    null,
                    Conversions.Default,
                    Customizations.None,
                    debuggerAttributes);
        }

        public class Conversion
        {
            [Fact]
            public void Local_beats_global_when_specified()
            {
                // create a dummy INamedTypeSymbol
                INamedTypeSymbol defaultType = new DummyNamedTypeSymbol();  
                var result = IntellenumConfiguration.Combine(
                    ConfigWithOmitConversionsAs(Conversions.EfCoreValueConverter),
                    ConfigWithOmitConversionsAs(Conversions.NewtonsoftJson),
                    () => defaultType);

                result.Conversions.Should().Be(Conversions.EfCoreValueConverter);
            }

            private static IntellenumConfiguration ConfigWithOmitConversionsAs(Conversions conversions) =>
                new IntellenumConfiguration(
                    null,
                    conversions,
                    Customizations.None,
                    DebuggerAttributeGeneration.Full);
        }
    }
}