using System;


namespace R5T.D0038.A001
{
    public static class IServiceAggregationIncrementExtensions
    {
        public static T FillFrom<T>(this T aggregation,
            IServiceAggregationIncrement other)
            where T : IServiceAggregationIncrement
        {
            aggregation.LibGit2SharpOperatorAction = other.LibGit2SharpOperatorAction;

            return aggregation;
        }
    }
}
