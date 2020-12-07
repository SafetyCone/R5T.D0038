using System;
using System.Linq;
using System.Threading.Tasks;

using LibGit2Sharp;
using LibGit2Sharp.Handlers;

using R5T.D0046;
using R5T.L0001;
using R5T.T0008;
using R5T.T0010;


namespace R5T.D0038.L0001
{
    public class LibGit2SharpOperator : ILibGit2SharpOperator
    {
        private IGitAuthenticationProvider GitAuthenticationProvider { get; }


        public LibGit2SharpOperator(
            IGitAuthenticationProvider gitAuthenticationProvider)
        {
            this.GitAuthenticationProvider = gitAuthenticationProvider;
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

            using (var repository = new Repository(localRepositoryDirectoryPath.Value))
            {
                var remote = repository.Network.Remotes[GitHelper.OriginRemoteName];
                var refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);

                Commands.Fetch(repository, remote.Name, refSpecs, fetchOptions, String.Empty);
            }
        }

        public Task<RevisionIdentity> GetLatestLocalMasterRevision(LocalRepositoryContainedPath path)
        {
            var repositoryPath = RepositoryHelper.DiscoverRepositoryPath(path.Value);

            using (var repository = new Repository(repositoryPath))
            {
                var masterBranch = repository.Branches[GitHelper.MasterBranchName];

                var tipCommit = masterBranch.Tip;

                var revisionIdentity = RevisionIdentity.From(tipCommit.Sha);

                return Task.FromResult(revisionIdentity);
            }
        }

        public Task<RemoteRepositoryUrl> GetRemoteOriginUrl(LocalRepositoryContainedPath path)
        {
            var repositoryPath = RepositoryHelper.DiscoverRepositoryPath(path.Value);

            using (var repository = new Repository(repositoryPath))
            {
                var remoteOrigin = repository.Network.Remotes[GitHelper.OriginRemoteName];

                var remoteRepositoryUrl = RemoteRepositoryUrl.From(remoteOrigin.Url);

                return Task.FromResult(remoteRepositoryUrl);
            }
        }

        public Task<bool> HasUnpulledMasterBranchChanges(LocalRepositoryDirectoryPath repositoryDirectoryPath)
        {
            using (var repository = new Repository(repositoryDirectoryPath.Value))
            {
                // Determine if the local master branch is behind the remote master branch.
                var masterBranch = repository.Branches[GitHelper.MasterBranchName];
                if(masterBranch is null)
                {
                    masterBranch = repository.Branches["main"];
                }

                var isBehind = masterBranch.TrackingDetails.BehindBy;

                var hasUnpulledMasterBranchChanges = isBehind.HasValue && isBehind.Value > 0;

                return Task.FromResult(hasUnpulledMasterBranchChanges);
            }
        }

        public Task<bool> HasUnpushedLocalChanges(LocalRepositoryDirectoryPath repositoryDirectoryPath)
        {
            using (var repository = new Repository(repositoryDirectoryPath.Value))
            {
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
        }
    }
}
