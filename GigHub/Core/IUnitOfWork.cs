using GigHub.Core.IRepositories;

namespace GigHub.Core
{
    /*
     * Abstractions should not depend on details. Details should depend on abstractions:
     *
     * IUnitOfWork is an abstraction that holds no dependencies to any details (Repository's concrete implementation),
     * it only references Repository interfaces, which in turn have no direct dependency to any database system's concrete
     * implementations. 
     */
    public interface IUnitOfWork
    {
        //Repositories:
        IGigRepository IRepoGigs { get; }
        IAttendanceRepository IRepoAttendance { get; }
        IGenresRepository IRepoGenres { get; }
        IFollowingsRepository IRepoFollowings { get; }

        //Methods
        void Complete();
    }
}