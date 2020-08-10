using System;

using Microsoft.Extensions.DependencyInjection;

using R5T.Dacia;


namespace R5T.D0038.L0001
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the <see cref="LibGit2SharpOperator"/> implementation of <see cref="ILibGit2SharpOperator"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceCollection AddLibGit2SharpOperator(this IServiceCollection services)
        {
            services.AddSingleton<ILibGit2SharpOperator, LibGit2SharpOperator>();

            return services;
        }

        /// <summary>
        /// Adds the <see cref="LibGit2SharpOperator"/> implementation of <see cref="ILibGit2SharpOperator"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceAction<ILibGit2SharpOperator> AddLibGit2SharpOperatorAction(this IServiceCollection services)
        {
            var serviceAction = ServiceAction.New<ILibGit2SharpOperator>(() => services.AddLibGit2SharpOperator());
            return serviceAction;
        }
    }
}
