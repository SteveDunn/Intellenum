using Intellenum.Tests.Types;

namespace ConsumerTests.ToStringTests;

public class BasicFunctionality
{
    [Fact]
    public void ToString_uses_generated_method()
    {
        Age.LegalVotingAge.ToString().Should().Be("LegalVotingAge");
        Age.LegalDrivingAge.ToString().Should().Be("LegalDrivingAge");

        NameType.FirstAndLast.ToString().Should().Be("FirstAndLast");
        NameType.Nickname.ToString().Should().Be("Nickname");
    }
}