using System;

using R5T.D0046;
using R5T.T0062;
using R5T.T0063;


namespace R5T.D0038.L0001
{
    public static class IServiceActionExtensions
    {
        /// <summary>
        /// Adds the <see cref="LibGit2SharpOperator"/> implementation of <see cref="ILibGit2SharpOperator"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceAction<ILibGit2SharpOperator> AddLibGit2SharpOperatorAction(this IServiceAction _,
            IServiceAction<IGitAuthenticationProvider> gitAuthenticationProviderAction,
            IServiceAction<IGitAuthorProvider> gitAuthorProviderAction)
        {
            var serviceAction = _.New<ILibGit2SharpOperator>(services => services.AddLibGit2SharpOperator(
                gitAuthenticationProviderAction,
                gitAuthorProviderAction));

            return serviceAction;
        }
    }
}
