﻿using System;

using Microsoft.Extensions.DependencyInjection;

using R5T.Dacia;

using R5T.D0046;


namespace R5T.D0038.L0001
{
    public static partial class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the <see cref="LibGit2SharpOperator"/> implementation of <see cref="ILibGit2SharpOperator"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceCollection AddLibGit2SharpOperator(this IServiceCollection services,
            IServiceAction<IGitAuthenticationProvider> gitAuthenticationProviderAction,
            IServiceAction<IGitAuthorProvider> gitAuthorProviderAction)
        {
            services
                .AddSingleton<ILibGit2SharpOperator, LibGit2SharpOperator>()
                .Run(gitAuthenticationProviderAction)
                .Run(gitAuthorProviderAction)
                ;

            return services;
        }

        /// <summary>
        /// Adds the <see cref="LibGit2SharpOperator"/> implementation of <see cref="ILibGit2SharpOperator"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceAction<ILibGit2SharpOperator> AddLibGit2SharpOperatorAction(this IServiceCollection services,
            IServiceAction<IGitAuthenticationProvider> gitAuthenticationProviderAction,
            IServiceAction<IGitAuthorProvider> gitAuthorProviderAction)
        {
            var serviceAction = ServiceAction.New<ILibGit2SharpOperator>(() => services.AddLibGit2SharpOperator(
                gitAuthenticationProviderAction,
                gitAuthorProviderAction));

            return serviceAction;
        }
    }
}
