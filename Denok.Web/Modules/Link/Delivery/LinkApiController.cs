using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace Denok.Web.Modules.Link.Delivery
{
    [ApiController]
    [Route("api/links")]
    public class LinkApiController : ControllerBase
    {
        private readonly ILogger<LinkApiController> _logger;
        private readonly Modules.Link.Usecase.ILinkUsecase _linkUsecase;

        public LinkApiController(ILogger<LinkApiController> logger, 
            Modules.Link.Usecase.ILinkUsecase linkUsecase)
        {
            _logger = logger;
            _linkUsecase = linkUsecase;
        }

        [HttpPost("generate")]
        [Consumes("application/json")]
        public async Task<IActionResult> Generate([FromBody] Modules.Link.Model.GenerateRequest generateRequest)
        {
            if (generateRequest == null)
            {
                return BadRequest(
                    new Denok.Lib.Shared.Response<Lib.Shared.EmptyJson> (
                        success: false,
                        code: 400,
                        message: "invalid request body",
                        data: new Lib.Shared.EmptyJson()
                    )
                );
            }

            Uri uriResult;
            bool validUrl = Utils.Utils.ValidHttpURL(generateRequest.OriginalLink, out uriResult);

            if (!validUrl)
            {
                return BadRequest(
                    new Denok.Lib.Shared.Response<Lib.Shared.EmptyJson> (
                        success: false,
                        code: 400,
                        message: "invalid input url",
                        data: new Lib.Shared.EmptyJson()
                    )
                );
            }

            var domainName = Config.AppConfig.DomainName;
            var outputLink = Lib.LinkGenerator.LinkGenerator.Generate();

            // save
            var linkRequest = new Model.LinkRequest();
            linkRequest.CreatedBy = "system";
            linkRequest.OriginalLink = uriResult.AbsoluteUri;
            linkRequest.GeneratedLink = outputLink;
            if (generateRequest.Description != null)
            {
                linkRequest.Description = generateRequest.Description;
            }
            linkRequest.TotalVisits = 0;

            var saveLinkResult = await _linkUsecase.CreateLink(linkRequest);
            if (saveLinkResult.IsError()) 
            {
                return BadRequest(
                    new Denok.Lib.Shared.Response<Lib.Shared.EmptyJson> (
                        success: false,
                        code: 400,
                        message: "error create link",
                        data: new Lib.Shared.EmptyJson()
                    )
                );
            }
            
            var linkResponse = saveLinkResult.Get();
            return Ok(
                new Denok.Lib.Shared.Response<Model.LinkResponse> (
                    success: true,
                    code: 200,
                    message: "create new link succeed",
                    data: linkResponse
                )
            );
        }

        [HttpPost("generate-custom")]
        [Consumes("application/json")]
        public async Task<IActionResult> GenerateCustom([FromBody] Modules.Link.Model.CustomLinkRequest generateRequest)
        {
            if (generateRequest == null)
            {
                return BadRequest(
                    new Denok.Lib.Shared.Response<Lib.Shared.EmptyJson> (
                        success: false,
                        code: 400,
                        message: "invalid request body",
                        data: new Lib.Shared.EmptyJson()
                    )
                );
            }

            // check custom link existence
            var findByCustomLinkResult = await _linkUsecase.GetLinkByGeneratedLink(generateRequest.CustomLink);
            if (!findByCustomLinkResult.IsError() && !findByCustomLinkResult.IsEmpty())
            {
                return BadRequest(
                new Denok.Lib.Shared.Response<Lib.Shared.EmptyJson> (
                    success: false,
                    code: 400,
                    message: "your custom link already exist",
                    data: new Lib.Shared.EmptyJson()
                )
            );
            }
            
            // check original lik
            Uri uriResult;
            bool validUrl = Utils.Utils.ValidHttpURL(generateRequest.OriginalLink, out uriResult);

            if (!validUrl)
            {
                return BadRequest(
                    new Denok.Lib.Shared.Response<Lib.Shared.EmptyJson> (
                        success: false,
                        code: 400,
                        message: "invalid input url",
                        data: new Lib.Shared.EmptyJson()
                    )
                );
            }

            var domainName = Config.AppConfig.DomainName;
            var outputLink = generateRequest.CustomLink;

            if (Utils.Utils.HasInvalidUriChar(outputLink))
            {
                return BadRequest(
                new Denok.Lib.Shared.Response<Lib.Shared.EmptyJson> (
                    success: false,
                    code: 400,
                    message: "invalid input url",
                    data: new Lib.Shared.EmptyJson()
                )
            );
            }
            
            var customLink = String.Format("{0}/{1}", domainName, outputLink);

            Uri uriCustomResult;
            bool validCustomUrl = Utils.Utils.ValidHttpURL(customLink, out uriCustomResult);

            if (!validCustomUrl)
            {
                return BadRequest(
                    new Denok.Lib.Shared.Response<Lib.Shared.EmptyJson> (
                        success: false,
                        code: 400,
                        message: "invalid custom url",
                        data: new Lib.Shared.EmptyJson()
                    )
                );
            }

            // save
            var linkRequest = new Model.LinkRequest();
            linkRequest.CreatedBy = "system";
            linkRequest.OriginalLink = uriResult.AbsoluteUri;
            linkRequest.GeneratedLink = outputLink;
            if (generateRequest.Description != null)
            {
                linkRequest.Description = generateRequest.Description;
            }
            linkRequest.TotalVisits = 0;

            var saveLinkResult = await _linkUsecase.CreateLink(linkRequest);
            if (saveLinkResult.IsError()) 
            {
                return BadRequest(
                    new Denok.Lib.Shared.Response<Lib.Shared.EmptyJson> (
                        success: false,
                        code: 400,
                        message: "error save generated link",
                        data: new Lib.Shared.EmptyJson()
                    )
                );
            }
            
            var linkResponse = saveLinkResult.Get();
            return Ok(
                new Denok.Lib.Shared.Response<Model.LinkResponse> (
                    success: true,
                    code: 200,
                    message: "create new custom link succeed",
                    data: linkResponse
                )
            );
        }
    }
}