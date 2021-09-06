using System;


namespace R5T.D0038.A001
{
    public static class IServiceAggregationExtensions
    {
        public static T FillFrom<T>(this T aggregation,
            IServiceAggregation other)
            where T : IServiceAggregation
        {
            (aggregation as D0046.A001.IServiceAggregation).FillFrom(other);

            aggregation.LibGit2SharpOperatorAction = other.LibGit2SharpOperatorAction;

            return aggregation;
        }
    }
}
