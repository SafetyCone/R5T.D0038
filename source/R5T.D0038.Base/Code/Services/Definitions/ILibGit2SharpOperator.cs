using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using R5T.T0010;
using R5T.T0064;


namespace R5T.D0038
{
    /// <summary>
    /// Service for the LibGit2Sharp 3rd party library.
    /// </summary>
    /// <remarks>
    /// Service is asynchronous even though the underlying LibGit2Sharp library is synchronous to better match possible internal service impedances.
    /// This is done to critically avoid synchronous-over-asynchronous.
    /// </remarks>
    [ServiceDefinitionMarker]
    public interface ILibGit2SharpOperator : IServiceDefinition
    {
        Task<string> CloneNonIdempotent(
            string sourceUrl,
            LocalRepositoryDirectoryPath localRepositoryDirectoryPath);

        Task Commit(LocalRepositoryDirectoryPath localRepositoryDirectoryPath,
            string commitMessage);

        Task Fetch(LocalRepositoryDirectoryPath localRepositoryDirectoryPath);

        Task<bool> HasUnpushedLocalChanges(LocalRepositoryDirectoryPath repositoryDirectoryPath);

        /// <summary>
        /// Determines if a local repository's master branch is missing commits that exist the remote repository.
        /// </summary>
        Task<bool> HasUnpulledMasterBranchChanges(LocalRepositoryDirectoryPath repositoryDirectoryPath);

        Task<RemoteRepositoryUrl> GetRemoteOriginUrl(LocalRepositoryContainedPath path);

        /// <summary>
        /// Gets the latest revision for the master branch of the local repository containing the file or directory path, *not* the file or directory itself.
        /// </summary>
        Task<RevisionIdentity> GetLatestLocalMasterRevision(LocalRepositoryContainedPath path);

        Task<string[]> ListAllUnstagedPaths(LocalRepositoryDirectoryPath localRepositoryDirectoryPath);

        Task Push(LocalRepositoryDirectoryPath localRepositoryDirectoryPath);

        Task Stage(LocalRepositoryDirectoryPath localRepositoryDirectoryPath,
            IEnumerable<string> filePaths);
    }
}
