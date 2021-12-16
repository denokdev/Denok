using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Denok.Lib.Shared;

namespace Denok.Web.Modules.Link.Repository
{
    public interface ILinkRepository
    {
        Task<Result<Model.Link, string>> Save(Model.Link user);

        Task<Result<Model.Link, string>> Update(string generatedLink, Model.Link user);
        
        Task<Result<Model.Link, string>> FindById(string id);

        Task<Result<Model.Link, string>> FindByGeneratedLink(string generatedLink);

        Task<Result<List<Model.Link>, string>> FindAll(Model.LinkFilter linkFilter);

        Task<Result<long, string>> Count(Model.LinkFilter linkFilter);
    }
}