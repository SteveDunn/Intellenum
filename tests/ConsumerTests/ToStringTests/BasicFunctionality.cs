using Intellenum.Tests.Types;

namespace ConsumerTests.ToStringTests;

public class BasicFunctionality
{
    [Fact]
    public void ToString_uses_generated_method()
    {
        MilestoneAges.LegalVotingAge.ToString().Should().Be("LegalVotingAge");
        MilestoneAges.LegalDrivingAge.ToString().Should().Be("LegalDrivingAge");

        NameType.FirstAndLast.ToString().Should().Be("FirstAndLast");
        NameType.Nickname.ToString().Should().Be("Nickname");
    }

    [Fact]
    public void Uses_users_method_if_supplied()
    {
        PacManPoints.Dot.ToString().Should().Be("00010");
        PacManPoints.PowerPellet.ToString().Should().Be("00050");
    }
}