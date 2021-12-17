using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Denok.Lib.Shared;

namespace Denok.Web.Modules.User.Repository
{
    public interface IUserRepository
    {
        Task<Result<Model.User, string>> Save(Model.User user);

        Task<Result<long, string>> Remove(string id);
        
        Task<Result<Model.User, string>> FindById(string id);

        Task<Result<Model.User, string>> FindByEmail(string email);

        Task<Result<Model.User, string>> FindByUsername(string username);

        Task<Result<List<Model.User>, string>> FindAll(Model.UserFilter userFilter);

        Task<Result<long, string>> Count(Model.UserFilter userFilter);
    }
}