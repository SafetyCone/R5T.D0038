using System;

using R5T.D0046;
using R5T.T0063;


namespace R5T.D0038.A002
{
    public class ServiceActionAggregation : IServiceActionAggregation
    {
        public IServiceAction<IGitAuthenticationProvider> GitAuthenticationProviderAction { get; set; }
        public IServiceAction<IGitAuthorProvider> GitAuthorProviderAction { get; set; }
        public IServiceAction<ILibGit2SharpOperator> LibGit2SharpOperatorAction { get; set; }
    }
}
