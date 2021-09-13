using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using R5T.Dacia;

using R5T.D0046.Default;
using R5T.D0046.I001;
using R5T.D0082.D001.I001;
using R5T.T0027.T008;

using R5T.D0038.L0001;


namespace R5T.D0038.Construction
{
    class Startup : T0027.T009.Startup
    {
        public Startup(ILogger<Startup> logger)
            : base(logger)
        {
        }

        public override async Task ConfigureConfiguration(
            IConfigurationBuilder configurationBuilder,
            IServiceProvider startupServicesProvider)
        {
            await base.ConfigureConfiguration(configurationBuilder, startupServicesProvider);
        }

        protected override async Task ConfigureServicesWithProvidedServices(
            IServiceCollection services,
            IServiceAction<IConfiguration> configurationAction,
            IServiceProvider startupServicesProvider,
            IProvidedServices providedServices)
        {
            await base.ConfigureServicesWithProvidedServices(
                services,
                configurationAction,
                startupServicesProvider,
                providedServices);

            // Services.
            // Level 1.
            var gitHubAuthenticationProviderAction = services.AddGitHubAuthenticationProviderAction(
                providedServices.SecretsDirectoryFilePathProviderAction);
            var gitAuthorProviderAction = services.AddGitAuthorProviderAction(
                providedServices.SecretsDirectoryFilePathProviderAction);

            // Level 2.
            var gitAuthenticationProviderAction = services.AddGitAuthenticationProviderAction(
                gitHubAuthenticationProviderAction);

            // Level 3.
            var libGit2SharpOperatorAction = services.AddLibGit2SharpOperatorAction(
                gitAuthenticationProviderAction,
                gitAuthorProviderAction);

            // Operations.
            // Level 1.

            services
                .Run(libGit2SharpOperatorAction)
                ;
        }
    }
}
