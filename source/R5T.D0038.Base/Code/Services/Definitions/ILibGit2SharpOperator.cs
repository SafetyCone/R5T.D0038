using System;

using R5T.T0010;


namespace R5T.D0038
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Service is synchronous because the underlying LibGit2Sharp library is synchronous.
    /// </remarks>
    public interface ILibGit2SharpOperator
    {
        bool HasUnpushedLocalChanges(LocalRepositoryDirectoryPath repositoryDirectoryPath);

        /// <summary>
        /// Determines if a local repository's master branch is missing commits that exist the remote repository.
        /// </summary>
        bool HasUnpulledMasterBranchChanges(LocalRepositoryDirectoryPath repositoryDirectoryPath);

        RemoteRepositoryUrl GetRemoteOriginUrl(LocalRepositoryContainedPath path);

        /// <summary>
        /// Gets the latest revision for the master branch of the local repository containing the file or directory path, *not* the file or directory itself.
        /// </summary>
        RevisionIdentity GetLatestLocalMasterRevision(LocalRepositoryContainedPath path);
    }
}
