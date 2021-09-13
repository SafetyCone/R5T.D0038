using System;

using Microsoft.Extensions.DependencyInjection;

using R5T.Dacia;
using R5T.Suebia;

using R5T.D0038.L0001;
using R5T.D0046.Default;
using R5T.D0046.I001;
using R5T.D0082.D001;


namespace R5T.D0038.A002
{
    public static class IServiceCollectionExtensions
    {
        public static ServiceAggregation AddLibGit2SharpOperatorServiceActions(this IServiceCollection services,
            IServiceAction<IGitHubAuthenticationProvider> gitHubAuthenticationProviderAction,
            IServiceAction<ISecretsDirectoryFilePathProvider> secretsDirectoryFilePathProviderAction)
        {
            // Level 1.
            var gitAuthenticationProviderAction = services.AddGitAuthenticationProviderAction(
                gitHubAuthenticationProviderAction);
            var gitAuthorProviderAction = services.AddGitAuthorProviderAction(
                secretsDirectoryFilePathProviderAction);

            // Level 2.
            var libGit2SharpOperatorAction = services.AddLibGit2SharpOperatorAction(
                gitAuthenticationProviderAction,
                gitAuthorProviderAction);

            return new ServiceAggregation
            {
                GitAuthenticationProviderAction = gitAuthenticationProviderAction,
                GitAuthorProviderAction = gitAuthorProviderAction,
                LibGit2SharpOperatorAction = libGit2SharpOperatorAction,
            };
        }
    }
}
