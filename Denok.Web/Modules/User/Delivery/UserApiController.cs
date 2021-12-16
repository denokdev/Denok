using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Denok.Web.Modules.User.Delivery
{
    [ApiController]
    [Route("api/users")]
    public class UserApiController : ControllerBase
    {
        private readonly ILogger<UserApiController> _logger;
        private readonly Usecase.IUserUsecase _userUsecase;

        public UserApiController(ILogger<UserApiController> logger, Usecase.IUserUsecase userUsecase)
        {
            _logger = logger;
            _userUsecase = userUsecase;
        }

        [HttpPost("")]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateUser([FromBody] Model.UserRequest userRequest)
        {
            if (userRequest == null)
            {
                return BadRequest(
                    new Denok.Lib.Shared.Response<Lib.Shared.EmptyJson> (
                        success: false,
                        code: 400,
                        message: "create user returned error",
                        data: new Lib.Shared.EmptyJson()
                    )
                );
            }

            var createResult = await _userUsecase.CreateUser(userRequest);
            if (createResult.IsError())
            {
                return BadRequest(
                    new Denok.Lib.Shared.Response<Lib.Shared.EmptyJson> (
                        success: false,
                        code: 400,
                        message: createResult.Error(),
                        data: new Lib.Shared.EmptyJson()
                    )
                );
            }

            var user = createResult.Get();
            return Created(user.Id,
                new Denok.Lib.Shared.Response<Model.UserResponse> (
                    success: true,
                    code: 201,
                    message: "create user succeed",
                    data: user
                )
            );
        }
    }
}