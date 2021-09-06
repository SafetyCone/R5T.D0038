using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using R5T.Plymouth;
using R5T.Plymouth.ProgramAsAService;

using R5T.T0010;


namespace R5T.D0038.Construction
{
    class Program : ProgramAsAServiceBase
    {
        #region Main

        static Task Main()
        {
            return ApplicationBuilder.Instance
                .NewApplication()
                .UseProgramAsAService<Program>()
                .UseT0027_T009_TwoStageStartup<Startup>()
                .BuildProgramAsAServiceHost()
                .Run();
        }

        #endregion


        private IServiceProvider ServiceProvider { get; }


        public Program(IApplicationLifetime applicationLifetime,
            IServiceProvider serviceProvider)
            : base(applicationLifetime)
        {
            this.ServiceProvider = serviceProvider;
        }

        protected override Task ServiceMain(CancellationToken stoppingToken)
        {
            return this.RunMethod();
            //return this.RunOperation();
        }

        private Task RunMethod()
        {
            return this.Test_CloneRemoteRepository();
        }

        /// <summary>
        /// Awesome! It works exactly the way you think it would, creating directories and everything.
        /// </summary>
        public async Task Test_CloneRemoteRepository()
        {
            var remoteRepositoryUrl = @"https://github.com/SafetyCone/R5T.Lombardy.git";
            var localRepositoryDirectoryPath = @"C:\Temp\Repos\R5T.Lombardy";

            var libGit2SharpOperator = this.ServiceProvider.GetRequiredService<ILibGit2SharpOperator>();

            await libGit2SharpOperator.Clone(
                remoteRepositoryUrl,
                new LocalRepositoryDirectoryPath(localRepositoryDirectoryPath));
        }

        //private async Task RunOperation()
        //{
        //    //await this.ServiceProvider.Run<O010_Test_AddNewServiceImplementationLibraryToSolution>();
        //}
    }
}
