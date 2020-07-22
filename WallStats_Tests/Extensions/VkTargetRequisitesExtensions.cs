using WallStats;

namespace WallStats_Tests.Extensions
{
    public static class VkTargetRequisitesExtensions
    {
        public static VkTargetRequisitesAssertions Should(this Target requisites)
        {
            return new VkTargetRequisitesAssertions(requisites);
        }
    }
}