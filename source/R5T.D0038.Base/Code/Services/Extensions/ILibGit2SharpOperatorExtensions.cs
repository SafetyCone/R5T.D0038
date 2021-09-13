using System;
using System.IO;
using System.Threading.Tasks;

using R5T.D0038;
using R5T.T0010;


namespace System
{
    public static class ILibGit2SharpOperatorExtensions
    {
        /// <summary>
        /// Check-in is:
        ///     1) Stage all unstaged files.
        ///     2) Commit the staged changes.
        ///     3) Push the committed changes of the head branch to the remote.
        /// </summary>
        public static async Task CheckIn(this ILibGit2SharpOperator libGit2SharpOperator,
            LocalRepositoryDirectoryPath localRepositoryDirectoryPath,
            string commitMessage)
        {
            await libGit2SharpOperator.StageAllUnstagedPaths(localRepositoryDirectoryPath);

            await libGit2SharpOperator.Commit(localRepositoryDirectoryPath, commitMessage);

            await libGit2SharpOperator.Push(localRepositoryDirectoryPath);
        }

        /// <summary>
        /// In general, do not use.
        /// </summary>
        public static async Task<string> CloneIdempotent(this ILibGit2SharpOperator libGit2SharpOperator,
            string sourceUrl,
            LocalRepositoryDirectoryPath localRepositoryDirectoryPath)
        {
            if(Directory.Exists(localRepositoryDirectoryPath.Value))
            {
                return localRepositoryDirectoryPath.Value;
            }
            else
            {
                var output = await libGit2SharpOperator.CloneNonIdempotent(
                    sourceUrl,
                    localRepositoryDirectoryPath);

                return output;
            }
        }

        public static async Task StageAllUnstagedPaths(this ILibGit2SharpOperator libGit2SharpOperator,
            LocalRepositoryDirectoryPath localRepositoryDirectoryPath)
        {
            var unstagedPaths = await libGit2SharpOperator.ListAllUnstagedPaths(localRepositoryDirectoryPath);

            await libGit2SharpOperator.Stage(localRepositoryDirectoryPath,
                unstagedPaths);
        }
    }
}
