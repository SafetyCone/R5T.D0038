using System;

using R5T.D0038.A002;


namespace System
{
    public static class IServiceAggregationExtensions
    {
        public static T FillFrom<T>(this T aggregation,
            IServiceAggregation other)
            where T : IServiceAggregation
        {
            aggregation.GitAuthenticationProviderAction = other.GitAuthenticationProviderAction;
            aggregation.GitAuthorProviderAction = other.GitAuthorProviderAction;
            aggregation.LibGit2SharpOperatorAction = other.LibGit2SharpOperatorAction;

            return aggregation;
        }
    }
}
