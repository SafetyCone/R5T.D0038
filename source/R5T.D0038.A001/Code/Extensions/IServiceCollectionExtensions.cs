using System;

using Microsoft.Extensions.DependencyInjection;

using R5T.Dacia;
using R5T.Suebia;

using R5T.D0038.L0001;
using R5T.D0046;
using R5T.D0046.A001;


namespace R5T.D0038.A001
{
    public static class IServiceCollectionExtensions
    {
        public static ServiceAggregation AddLibGit2SharpOperatorServiceActions(this IServiceCollection services,
            IServiceAction<IGitAuthorProvider> gitAuthorProviderAction,
            IServiceAction<ISecretsDirectoryFilePathProvider> secretsDirectoryFilePathProviderAction)
        {
            var gitAuthenticationProviderServiceActions = services.AddGitAuthenticationProviderServiceActions(
                secretsDirectoryFilePathProviderAction);

            var libGit2SharpOperatorAction = services.AddLibGit2SharpOperatorAction(
                gitAuthenticationProviderServiceActions.GitAuthenticationProviderAction,
                gitAuthorProviderAction);

            var output = new ServiceAggregation
            {
                LibGit2SharpOperatorAction = libGit2SharpOperatorAction,
            }
            .FillFrom(gitAuthenticationProviderServiceActions)
            ;

            return output;
        }
    }
}
