using System;

using R5T.Dacia;

using R5T.D0046;


namespace R5T.D0038.A002
{
    public interface IServiceAggregation
    {
        IServiceAction<IGitAuthenticationProvider> GitAuthenticationProviderAction { get; set; }
        IServiceAction<ILibGit2SharpOperator> LibGit2SharpOperatorAction { get; set; }
    }
}
