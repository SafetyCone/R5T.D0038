using System;

using R5T.Suebia;

using R5T.D0038.L0001;
using R5T.D0046.Default;
using R5T.D0046.I001;
using R5T.D0082.D001;
using R5T.T0062;
using R5T.T0063;


namespace R5T.D0038.A002
{
    public static class IServiceActionExtensions
    {
        public static ServiceActionAggregation AddLibGit2SharpOperatorServiceActions(this IServiceAction _,
            IServiceAction<IGitHubAuthenticationProvider> gitHubAuthenticationProviderAction,
            IServiceAction<ISecretsDirectoryFilePathProvider> secretsDirectoryFilePathProviderAction)
        {
            // Level 1.
            var gitAuthenticationProviderAction = _.AddGitAuthenticationProviderAction(
                gitHubAuthenticationProviderAction);
            var gitAuthorProviderAction = _.AddGitAuthorProviderAction(
                secretsDirectoryFilePathProviderAction);

            // Level 2.
            var libGit2SharpOperatorAction = _.AddLibGit2SharpOperatorAction(
                gitAuthenticationProviderAction,
                gitAuthorProviderAction);

            var output = new ServiceActionAggregation
            {
                GitAuthenticationProviderAction = gitAuthenticationProviderAction,
                GitAuthorProviderAction = gitAuthorProviderAction,
                LibGit2SharpOperatorAction = libGit2SharpOperatorAction,
            };

            return output;
        }
    }
}
