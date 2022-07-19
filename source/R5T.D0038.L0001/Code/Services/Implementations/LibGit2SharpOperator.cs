using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using LibGit2Sharp;
using LibGit2Sharp.Handlers;

using R5T.D0046;
using R5T.L0001;
using R5T.T0008;
using R5T.T0010;
using R5T.T0064;


namespace R5T.D0038.L0001
{
    [ServiceImplementationMarker]
    public class LibGit2SharpOperator : ILibGit2SharpOperator, IServiceImplementation
    {
        private IGitAuthenticationProvider GitAuthenticationProvider { get; }
        private IGitAuthorProvider GitAuthorProvider { get; }


        public LibGit2SharpOperator(
            IGitAuthenticationProvider gitAuthenticationProvider,
            IGitAuthorProvider gitAuthorProvider)
        {
            this.GitAuthenticationProvider = gitAuthenticationProvider;
            this.GitAuthorProvider = gitAuthorProvider;
        }

        public async Task<string> CloneNonIdempotent(
            string sourceUrl,
            LocalRepositoryDirectoryPath localRepositoryDirectoryPath)
        {
            var authentication = await this.GitAuthenticationProvider.GetGitAuthentication();

            var options = new CloneOptions
            {
                CredentialsProvider = new CredentialsHandler((url, usernameFromUrl, types) =>
                    new UsernamePasswordCredentials()
                    {
                        Username = authentication.Username,
                        Password = authentication.Password,
                    }),
            };

            var repositoryDirectoryPath = Repository.Clone(sourceUrl, localRepositoryDirectoryPath.Value, options);
            return repositoryDirectoryPath;
        }

        // Adapted from here: https://github.com/libgit2/libgit2sharp/wiki/git-fetch
        public async Task Fetch(LocalRepositoryDirectoryPath localRepositoryDirectoryPath)
        {
            var authentication = await this.GitAuthenticationProvider.GetGitAuthentication();

            var fetchOptions = new FetchOptions
            {
                CredentialsProvider = new CredentialsHandler((url, usernameFromUrl, types) =>
                    new UsernamePasswordCredentials()
                    {
                        Username = authentication.Username,
                        Password = authentication.Password,
                    })
            };

            using var repository = new Repository(localRepositoryDirectoryPath.Value);

            var remote = repository.Network.Remotes[GitHelper.OriginRemoteName];
            var refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);

            Commands.Fetch(repository, remote.Name, refSpecs, fetchOptions, String.Empty);
        }

        public Task<RevisionIdentity> GetLatestLocalMasterRevision(LocalRepositoryContainedPath path)
        {
            var repositoryPath = RepositoryHelper.DiscoverRepositoryPath(path.Value);

            using var repository = new Repository(repositoryPath);

            var masterBranch = repository.Branches[GitHelper.MasterBranchName];

            var tipCommit = masterBranch.Tip;

            var revisionIdentity = RevisionIdentity.From(tipCommit.Sha);

            return Task.FromResult(revisionIdentity);
        }

        public Task<RemoteRepositoryUrl> GetRemoteOriginUrl(LocalRepositoryContainedPath path)
        {
            var repositoryPath = RepositoryHelper.DiscoverRepositoryPath(path.Value);

            using var repository = new Repository(repositoryPath);

            var remoteOrigin = repository.Network.Remotes[GitHelper.OriginRemoteName];

            var remoteRepositoryUrl = RemoteRepositoryUrl.From(remoteOrigin.Url);

            return Task.FromResult(remoteRepositoryUrl);
        }

        public Task<bool> HasUnpulledMasterBranchChanges(LocalRepositoryDirectoryPath repositoryDirectoryPath)
        {
            using var repository = new Repository(repositoryDirectoryPath.Value);

            // Determine if the local master branch is behind the remote master branch.
            var masterBranch = repository.Branches[GitHelper.MasterBranchName];
            if (masterBranch is null)
            {
                masterBranch = repository.Branches["main"];
            }

            var isBehind = masterBranch.TrackingDetails.BehindBy;

            var hasUnpulledMasterBranchChanges = isBehind.HasValue && isBehind.Value > 0;

            return Task.FromResult(hasUnpulledMasterBranchChanges);
        }

        public Task<bool> HasUnpushedLocalChanges(LocalRepositoryDirectoryPath repositoryDirectoryPath)
        {
            using var repository = new Repository(repositoryDirectoryPath.Value);

            // Assume no unpushed changes.
            var hasUnPushedChanges = false;

            // Are there any untracked files? (Other than ignored files.)
            // => I think the below takes care of this.

            // Are there any unstaged or uncommitted changes?
            var treeChanges = repository.Diff.Compare<TreeChanges>(
                repository.Head.Tip.Tree,
                DiffTargets.Index | DiffTargets.WorkingDirectory);

            hasUnPushedChanges = treeChanges.Count > 0;
            if (hasUnPushedChanges)
            {
                return Task.FromResult(hasUnPushedChanges);
            }

            // Get the current branch.
            var currentBranch = repository.Head;

            // Is the current branch untracked? This indicates that it has not been pushed to the remote!
            var isUntracked = !currentBranch.IsTracking;

            hasUnPushedChanges = isUntracked;
            if (hasUnPushedChanges)
            {
                return Task.FromResult(hasUnPushedChanges);
            }

            // Is the current branch ahead its remote tracking branch?
            var currentBranchLocalIsAheadOfRemote = currentBranch.TrackingDetails.AheadBy > 0;

            hasUnPushedChanges = currentBranchLocalIsAheadOfRemote;
            if (hasUnPushedChanges)
            {
                return Task.FromResult(hasUnPushedChanges);
            }

            // Finally, return the originally assumed value, that there are no unpushed changes.
            return Task.FromResult(hasUnPushedChanges);
        }

        public Task Stage(LocalRepositoryDirectoryPath localRepositoryDirectoryPath,
            IEnumerable<string> filePaths)
        {
            using var repository = new Repository(localRepositoryDirectoryPath.Value);

            // Stage paths, if any.
            if(filePaths.Any())
            {
                Commands.Stage(repository, filePaths);
            }

            return Task.CompletedTask;
        }

        public async Task Commit(LocalRepositoryDirectoryPath localRepositoryDirectoryPath, string filePath, string commitMessage)
        {
            var author = await this.GitAuthorProvider.GetGitAuthor();

            var authorSignature = new Signature(author.Name, author.EmailAddress, DateTime.Now);
            var committerSignature = authorSignature;

            using var repository = new Repository(localRepositoryDirectoryPath.Value);

            Commands.Stage(repository, filePath);

            repository.Commit(
                commitMessage,
                authorSignature,
                committerSignature);
        }

        public async Task Commit(LocalRepositoryDirectoryPath localRepositoryDirectoryPath, string commitMessage)
        {
            var author = await this.GitAuthorProvider.GetGitAuthor();

            var authorSignature = new Signature(author.Name, author.EmailAddress, DateTime.Now);
            var committerSignature = authorSignature;

            using var repository = new Repository(localRepositoryDirectoryPath.Value);

            repository.Commit(
                commitMessage,
                authorSignature,
                committerSignature);
        }

        public Task<string[]> ListAllUnstagedPaths(LocalRepositoryDirectoryPath localRepositoryDirectoryPath)
        {
            using var repository = new Repository(localRepositoryDirectoryPath.Value);

            var unstagedPaths = repository.Diff.Compare<TreeChanges>(repository.Head.Tip.Tree, DiffTargets.WorkingDirectory)
                  .Select(xChange => xChange.Path)
                  .ToArray();

            return Task.FromResult(unstagedPaths);
        }

        /// <summary>
        /// Push the HEAD branch.
        /// </summary>
        public async Task Push(LocalRepositoryDirectoryPath localRepositoryDirectoryPath)
        {
            var authentication = await this.GitAuthenticationProvider.GetGitAuthentication();

            using var repository = new Repository(localRepositoryDirectoryPath.Value);

            var pushOptions = new PushOptions
            {
                CredentialsProvider = new CredentialsHandler((url, usernameFromUrl, types) =>
                    new UsernamePasswordCredentials()
                    {
                        Username = authentication.Username,
                        Password = authentication.Password,
                    })
            };

            repository.Network.Push(repository.Head, pushOptions);
        }

        public Task<bool> IsRepository(string directoryPath)
        {
            var output = Repository.IsValid(directoryPath);
            
            return Task.FromResult(output);
        }
    }
}
