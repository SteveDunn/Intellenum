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
using FluentAssertions;
using Intellenum.Tests.Types;

namespace ConsumerTests
{
    public class EqualityTests
    {
        [Fact]
        public void equality_between_same_value_objects()
        {
            Age.LegalVotingAge.Equals(Age.LegalVotingAge).Should().BeTrue();
            (Age.LegalVotingAge == Age.LegalVotingAge).Should().BeTrue();
            
            // uses the generated IEquatable<> 
            (Age.LegalVotingAge == 18).Should().BeTrue();
            (18 == Age.LegalVotingAge).Should().BeTrue();

            (Age.LegalVotingAge != Age.LegalDrivingAge).Should().BeTrue();
            (Age.LegalVotingAge == Age.LegalDrivingAge).Should().BeFalse();

            Age.LegalVotingAge.Equals(Age.LegalVotingAge).Should().BeTrue();
            (Age.LegalVotingAge == Age.LegalVotingAge).Should().BeTrue();
        }

        [Fact]
        public void equality_between_different_value_objects()
        {
            Age.LegalVotingAge.Equals(ScoreType.Points).Should().BeFalse();
            (Age.LegalVotingAge == (object)ScoreType.Points).Should().BeFalse();
        }

        [Fact]
        public void equality_with_primitives()
        {
            Age.LegalVotingAge.Equals(-1).Should().BeFalse();
            (Age.LegalVotingAge == 18).Should().BeTrue();
            (18 == Age.LegalVotingAge).Should().BeTrue();
            Age.LegalVotingAge.Equals(18).Should().BeTrue();

            (Age.LegalVotingAge != Age.LegalDrivingAge).Should().BeTrue();
            (Age.LegalVotingAge != 2).Should().BeTrue();
            (Age.LegalVotingAge == 2).Should().BeFalse();
            Age.LegalVotingAge.Equals(Age.LegalDrivingAge).Should().BeFalse();

            Age.LegalVotingAge.Equals(new StackFrame()).Should().BeFalse();
            
            Age.LegalVotingAge.Equals(ScoreType.Points).Should().BeFalse();
        }

        [Fact]
        public void reference_equality()
        {
            var age1 = Age.LegalVotingAge;
            var age2 = Age.LegalVotingAge;

            age1.Equals(age1).Should().BeTrue();

            object.ReferenceEquals(age1, age2).Should().BeTrue();
        }

        [Fact]
        public void equality_with_object()
        {
            var age = Age.LegalVotingAge;
            age.Equals((object)age).Should().BeTrue();
            
            age.Equals((object)"???").Should().BeFalse();

            (age == (object) "??").Should().BeFalse();
            (age != (object) "??").Should().BeTrue();
        }

        [Fact]
        public void Nullness()
        {
            (Age.LegalVotingAge == null!).Should().BeFalse();
            Age.LegalVotingAge!.Equals(null!).Should().BeFalse();

            (Age.LegalDrivingAge != null!).Should().BeTrue();
        }
    }
}