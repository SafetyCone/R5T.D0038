using System;

using R5T.Dacia;

using R5T.D0046;


namespace R5T.D0038.A002
{
    public class ServiceAggregation : IServiceAggregation
    {
        public IServiceAction<IGitAuthenticationProvider> GitAuthenticationProviderAction { get; set; }
        public IServiceAction<IGitAuthorProvider> GitAuthorProviderAction { get; set; }
        public IServiceAction<ILibGit2SharpOperator> LibGit2SharpOperatorAction { get; set; }
    }
}
