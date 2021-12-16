using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using Denok.Lib.Shared;

namespace Denok.Web.Modules.Link.Usecase
{

    public interface ILinkUsecase 
    {
        Task<Result<Model.LinkResponse, string>> CreateLink(Model.LinkRequest linkRequest);
        
        Task<Result<Model.LinkResponse, string>> GetLink(string id);

        Task<Result<Model.LinkResponse, string>> GetLinkByGeneratedLink(string generatedLink);

        Task<Result<Model.LinkListView, string>> GetLinks(Model.LinkFilter linkFilter);

    }


     public class LinkUsecase : ILinkUsecase 
    {
        private readonly ILogger<LinkUsecase> _logger;

        private readonly Repository.ILinkRepository _linkRepository;

        public LinkUsecase(ILogger<LinkUsecase> logger, 
                Repository.ILinkRepository linkRepository
            )
        {
            _logger = logger;
            _linkRepository = linkRepository;
        }

        public async Task<Result<Model.LinkResponse, string>> CreateLink(Model.LinkRequest linkRequest)
        {
            var link = linkRequest.ToLinkModel();

            var findByGeneratedLinkResult = await _linkRepository.FindByGeneratedLink(link.GeneratedLink);
            if (!findByGeneratedLinkResult.IsError() && !findByGeneratedLinkResult.IsEmpty())
            {
                return Result<Model.LinkResponse, string>.From(null, "link already exist");
            }

            var saveResult = await _linkRepository.Save(link);

            if (saveResult.IsError())
            {
                return Result<Model.LinkResponse, string>.From(null, saveResult.Error());
            }

            var linkResponse = new Model.LinkResponse(saveResult.Get());
            return Result<Model.LinkResponse, string>.From(linkResponse, null);
        }

        public async Task<Result<Model.LinkResponse, string>> GetLink(string id)
        {
            var findByIdResult = await _linkRepository.FindById(id);
            if (findByIdResult.IsError())
            {
                return Result<Model.LinkResponse, string>.From(null, findByIdResult.Error());
            }

            var linkResponse = new Model.LinkResponse(findByIdResult.Get());
            return Result<Model.LinkResponse, string>.From(linkResponse, null);
        }

        public async Task<Result<Model.LinkResponse, string>> GetLinkByGeneratedLink(string generatedLink)
        {
            var findByResult = await _linkRepository.FindByGeneratedLink(generatedLink);
            if (findByResult.IsError())
            {
                return Result<Model.LinkResponse, string>.From(null, findByResult.Error());
            }

            var linkResponse = new Model.LinkResponse(findByResult.Get());
            return Result<Model.LinkResponse, string>.From(linkResponse, null);
        }

        public async Task<Result<Model.LinkListView, string>> GetLinks(Model.LinkFilter linkFilter)
        {
            var findAllResult = await _linkRepository.FindAll(linkFilter);
            if (findAllResult.IsError())
            {
                return Result<Model.LinkListView, string>.From(null, findAllResult.Error());
            }

            var linkResponses = new List<Model.LinkResponse>();
            findAllResult.Get().ForEach(link => {
                var linkResponse = new Model.LinkResponse(link);
                linkResponses.Add(linkResponse);
            });

            var countResult = await _linkRepository.Count(linkFilter);
            if (countResult.IsError())
            {
                return Result<Model.LinkListView, string>.From(null, countResult.Error());
            }

            var linkListView = new Model.LinkListView();
            linkListView.LinkData = linkResponses;
            linkListView.Meta = new Lib.Shared.Meta((int)linkFilter.Page, (int)linkFilter.Limit, (int)countResult.Get());

            return Result<Model.LinkListView, string>.From(linkListView, null);
        }

    }
}