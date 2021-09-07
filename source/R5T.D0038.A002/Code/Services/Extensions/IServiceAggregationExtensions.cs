using System;

using Microsoft.Extensions.DependencyInjection;

using R5T.Dacia;

using R5T.D0038.L0001;
using R5T.D0046.Default;
using R5T.D0082.D001;


namespace R5T.D0038.A002
{
    public static class IServiceAggregationExtensions
    {
        public static ServiceAggregation AddLibGit2SharpOperatorServiceActions(this IServiceCollection services,
            IServiceAction<IGitHubAuthenticationProvider> gitHubAuthenticationProviderAction)
        {
            var gitAuthenticationProviderAction = services.AddGitAuthenticationProviderAction(
                gitHubAuthenticationProviderAction);

            var libGit2SharpOperatorAction = services.AddLibGit2SharpOperatorAction(
                gitAuthenticationProviderAction);

            return new ServiceAggregation
            {
                GitAuthenticationProviderAction = gitAuthenticationProviderAction,
                LibGit2SharpOperatorAction = libGit2SharpOperatorAction,
            };
        }
    }
}
