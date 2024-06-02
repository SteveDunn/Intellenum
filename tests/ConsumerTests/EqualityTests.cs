// ReSharper disable NullableWarningSuppressionIsUsed
// ReSharper disable EqualExpressionComparison
// ReSharper disable SuspiciousTypeConversion.Global
// ReSharper disable RedundantNameQualifier
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable RedundantCast
// ReSharper disable StringLiteralTypo
// ReSharper disable PossibleNullReferenceException
#pragma warning disable 252,253

// ReSharper disable EqualExpressionComparison
#pragma warning disable 1718

using System.Diagnostics;
using Intellenum.Tests.Types;

namespace ConsumerTests
{
    public class EqualityTests
    {
        [Fact]
        public void equality_between_same_value_objects()
        {
            MilestoneAges.LegalVotingAge.Equals(MilestoneAges.LegalVotingAge).Should().BeTrue();
            (MilestoneAges.LegalVotingAge == MilestoneAges.LegalVotingAge).Should().BeTrue();
            
            // uses the generated IEquatable<> 
            (MilestoneAges.LegalVotingAge == 18).Should().BeTrue();
            (18 == MilestoneAges.LegalVotingAge).Should().BeTrue();

            (MilestoneAges.LegalVotingAge != MilestoneAges.LegalDrivingAge).Should().BeTrue();
            (MilestoneAges.LegalVotingAge == MilestoneAges.LegalDrivingAge).Should().BeFalse();

            MilestoneAges.LegalVotingAge.Equals(MilestoneAges.LegalVotingAge).Should().BeTrue();
            (MilestoneAges.LegalVotingAge == MilestoneAges.LegalVotingAge).Should().BeTrue();
        }

        [Fact]
        public void equality_between_different_value_objects()
        {
            MilestoneAges.LegalVotingAge.Equals(ScoreType.Points).Should().BeFalse();
            (MilestoneAges.LegalVotingAge == (object)ScoreType.Points).Should().BeFalse();
        }

        [Fact]
        public void equals_method_with_the_underlying_is_always_false()
        {
            MilestoneAges.LegalVotingAge.Equals(-1).Should().BeFalse();
            (MilestoneAges.LegalVotingAge.Equals(18)).Should().BeFalse();
            (MilestoneAges.LegalVotingAge.Equals(null)).Should().BeFalse();
            MilestoneAges.LegalVotingAge!.Equals(MilestoneAges.LegalDrivingAge).Should().BeFalse();

            MilestoneAges.LegalVotingAge.Equals(new StackFrame()).Should().BeFalse();
            
            MilestoneAges.LegalVotingAge.Equals(ScoreType.Points).Should().BeFalse();
        }

        [Fact]
        public void equality_with_primitives()
        {
            MilestoneAges.LegalVotingAge.Equals(-1).Should().BeFalse();
            (MilestoneAges.LegalVotingAge == 18).Should().BeTrue();
            (18 == MilestoneAges.LegalVotingAge).Should().BeTrue();

            (MilestoneAges.LegalVotingAge != MilestoneAges.LegalDrivingAge).Should().BeTrue();
            (MilestoneAges.LegalVotingAge != 2).Should().BeTrue();
            (MilestoneAges.LegalVotingAge == 2).Should().BeFalse();
        }

        [Fact]
        public void reference_equality()
        {
            var age1 = MilestoneAges.LegalVotingAge;
            var age2 = MilestoneAges.LegalVotingAge;

            age1.Equals(age1).Should().BeTrue();

            object.ReferenceEquals(age1, age2).Should().BeTrue();
        }

        [Fact]
        public void equality_with_object()
        {
            var age = MilestoneAges.LegalVotingAge;
            age.Equals((object)age).Should().BeTrue();
            
            age.Equals((object)"???").Should().BeFalse();

            (age == (object) "??").Should().BeFalse();
            (age != (object) "??").Should().BeTrue();
        }

        [Fact]
        public void Nullness()
        {
            (MilestoneAges.LegalVotingAge == null!).Should().BeFalse();
            MilestoneAges.LegalVotingAge!.Equals(null!).Should().BeFalse();

            (MilestoneAges.LegalDrivingAge != null!).Should().BeTrue();
        }
    }
}