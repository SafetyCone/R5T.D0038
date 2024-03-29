﻿using System;

using Microsoft.Extensions.DependencyInjection;

using R5T.D0046;
using R5T.T0063;


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
    }
}
