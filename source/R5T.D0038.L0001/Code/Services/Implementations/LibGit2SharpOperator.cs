using System;

using LibGit2Sharp;

using R5T.L0001;
using R5T.T0008;
using R5T.T0010;


namespace R5T.D0038
{
    public class LibGit2SharpOperator : ILibGit2SharpOperator
    {
        public RevisionIdentity GetLatestLocalMasterRevision(LocalRepositoryContainedPath path)
        {
            var repositoryPath = RepositoryHelper.DiscoverRepositoryPath(path.Value);

            using (var repository = new Repository(repositoryPath))
            {
                var masterBranch = repository.Branches[GitHelper.MasterBranchName];

                var tipCommit = masterBranch.Tip;

                var revisionIdentity = RevisionIdentity.From(tipCommit.Sha);
                return revisionIdentity;
            }
        }

        public RemoteRepositoryUrl GetRemoteOriginUrl(LocalRepositoryContainedPath path)
        {
            var repositoryPath = RepositoryHelper.DiscoverRepositoryPath(path.Value);

            using (var repository = new Repository(repositoryPath))
            {
                var remoteOrigin = repository.Network.Remotes[GitHelper.OriginRemoteName];

                var remoteRepositoryUrl = RemoteRepositoryUrl.From(remoteOrigin.Url);
                return remoteRepositoryUrl;
            }
        }

        public bool HasUnpulledMasterBranchChanges(LocalRepositoryDirectoryPath repositoryDirectoryPath)
        {
            using (var repository = new Repository(repositoryDirectoryPath.Value))
            {
                // Determine if the local master branch is behind the remote master branch.
                var masterBranch = repository.Branches[GitHelper.MasterBranchName];

                var isBehind = masterBranch.TrackingDetails.BehindBy;

                var hasUnpulledMasterBranchChanges = isBehind.HasValue && isBehind.Value > 0;

                return hasUnpulledMasterBranchChanges;
            }
        }

        public bool HasUnpushedLocalChanges(LocalRepositoryDirectoryPath repositoryDirectoryPath)
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
                    return hasUnPushedChanges;
                }

                // Get the current branch.
                var currentBranch = repository.Head;

                // Is the current branch untracked? This indicates that it has not been pushed to the remote!
                var isUntracked = !currentBranch.IsTracking;

                hasUnPushedChanges = isUntracked;
                if (hasUnPushedChanges)
                {
                    return hasUnPushedChanges;
                }

                // Is the current branch ahead its remote tracking branch?
                var currentBranchLocalIsAheadOfRemote = currentBranch.TrackingDetails.AheadBy > 0;

                hasUnPushedChanges = currentBranchLocalIsAheadOfRemote;
                if (hasUnPushedChanges)
                {
                    return hasUnPushedChanges;
                }

                // Finally, return the originally assumed value, that there are no unpushed changes.
                return hasUnPushedChanges;
            }
        }
    }
}
