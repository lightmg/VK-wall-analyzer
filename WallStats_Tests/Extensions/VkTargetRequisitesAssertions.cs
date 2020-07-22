using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using WallStats;
using WallStats.Bot.Enums;

namespace WallStats_Tests.Extensions
{
    public class VkTargetRequisitesAssertions :
        ReferenceTypeAssertions<Target, VkTargetRequisitesAssertions>
    {
        public VkTargetRequisitesAssertions(Target instance)
        {
            Subject = instance;
        }

        protected override string Identifier => "directory";

        public AndConstraint<VkTargetRequisitesAssertions> BeWithTargetType(TargetType type)
        {
            Execute.Assertion
                .ForCondition(Subject.TargetType == type)
                .FailWith($"Expected TargetType {type}, but was {Subject.TargetType}");

            return new AndConstraint<VkTargetRequisitesAssertions>(this);
        }

        public AndConstraint<VkTargetRequisitesAssertions> BeWithId(long id)
        {
            Execute.Assertion
                .ForCondition(Subject.Id == id)
                .FailWith($"Expected Id {id}, but was {Subject.Id}");

            return new AndConstraint<VkTargetRequisitesAssertions>(this);
        }
    }
}