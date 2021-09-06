using System;

using R5T.Dacia;


namespace R5T.D0038.A001
{
    public interface IServiceAggregationIncrement
    {
        IServiceAction<ILibGit2SharpOperator> LibGit2SharpOperatorAction { get; set; }
    }
}
