using FluentAssertions;
using NUnit.Framework;
using WallStats;
using WallStats.Bot.Enums;
using WallStats.Bot.Input;
using WallStats_Tests.Extensions;

namespace WallStats_Tests.Input
{
    [TestFixture]
    public class RequisiteParserTests
    {
        private TargetIdParser parser;

        [SetUp]
        public void SetUp()
        {
            parser = new TargetIdParser();
        }

        [TestCase(1)]
        [TestCase(0)]
        [TestCase(20)]
        [TestCase(1234567890)]
        public void UserId_CanParseValid(long id)
        {
            AssertParser("id" + id, TargetType.User, id);
            AssertParser("public" + id, TargetType.Group, id);
            AssertParser("club" + id, TargetType.Group, id);
        }

        [TestCase("", "No id")]
        [TestCase("-1", "Negative id")]
        [TestCase("11aaa11", "Symbols in id")]
        [TestCase("11@$%11", "Special symbols in id")]
        public void UserId_CantParse(string badId, string caseName)
        {
            parser.TryParse("id" + badId, out _).Should().BeFalse($"Parser should not work in this case: {caseName}");
            parser.TryParse("group" + badId, out _).Should().BeFalse($"Parser should not work in this case: {caseName}");
            parser.TryParse("club" + badId, out _).Should().BeFalse($"Parser should not work in this case: {caseName}");
        }

        private void AssertParser(string input, TargetType targetType, long id)
        {
            parser.TryParse(input, out var result).Should().BeTrue();
            result.Should().NotBeNull();
            result.Should().BeWithTargetType(targetType);
            result.Should().BeWithId(id);
        }
    }
}