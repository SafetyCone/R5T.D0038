using System;

using R5T.D0038.A002;


namespace System
{
    public static class IServiceActionAggregationExtensions
    {
        public static T FillFrom<T>(this T aggregation,
            IServiceActionAggregation other)
            where T : IServiceActionAggregation
        {
            aggregation.GitAuthenticationProviderAction = other.GitAuthenticationProviderAction;
            aggregation.GitAuthorProviderAction = other.GitAuthorProviderAction;
            aggregation.LibGit2SharpOperatorAction = other.LibGit2SharpOperatorAction;

            return aggregation;
        }
    }
}
