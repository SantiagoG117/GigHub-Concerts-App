using System.Collections.Generic;
using GigHub.Core.Models;

namespace GigHub.Core.IRepositories
{
    public interface IGenresRepository
    {
        IList<Genre> GetGenres();
    }
}