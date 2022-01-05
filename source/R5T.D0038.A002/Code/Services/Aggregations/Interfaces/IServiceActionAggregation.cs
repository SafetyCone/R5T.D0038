using System;

using R5T.D0046;
using R5T.T0063;


namespace R5T.D0038.A002
{
    public interface IServiceActionAggregation
    {
        IServiceAction<IGitAuthenticationProvider> GitAuthenticationProviderAction { get; set; }
        IServiceAction<IGitAuthorProvider> GitAuthorProviderAction { get; set; }
        IServiceAction<ILibGit2SharpOperator> LibGit2SharpOperatorAction { get; set; }
    }
}
