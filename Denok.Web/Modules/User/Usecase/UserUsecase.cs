using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using Denok.Lib.Shared;
using Denok.Web.Modules.User.Model;

namespace Denok.Web.Modules.User.Usecase
{

    public interface IUserUsecase 
    {
        Task<Result<Model.UserResponse, string>> CreateUser(Model.UserRequest userRequest);
        
        Task<Result<Model.UserResponse, string>> GetProfile(string id);

        Task<Result<Model.UserResponse, string>> Login(Model.LoginRequest loginRequest);

        Task<Result<List<Model.UserResponse>, string>> GetUsers(Model.UserFilter userFilter);

    }


     public class UserUsecase : IUserUsecase 
    {
        private readonly ILogger<UserUsecase> _logger;

        private readonly Repository.IUserRepository _userRepository;

        public UserUsecase(ILogger<UserUsecase> logger, 
                Repository.IUserRepository userRepository
            )
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<Result<UserResponse, string>> CreateUser(UserRequest userRequest)
        {
            var user = userRequest.ToUserModel();

            var findByUsernameResult = await _userRepository.FindByUsername(user.Username);
            if (!findByUsernameResult.IsError() && !findByUsernameResult.IsEmpty())
            {
                return Result<UserResponse, string>.From(null, "username already exist");
            }

            var findByEmailResult = await _userRepository.FindByEmail(user.Email);
            if (!findByEmailResult.IsError() && !findByEmailResult.IsEmpty())
            {
                return Result<UserResponse, string>.From(null, "email already exist");
            }

            user.Status = "ACTIVE";
            var saveResult = await _userRepository.Save(user);

            if (saveResult.IsError())
            {
                return Result<UserResponse, string>.From(null, saveResult.Error());
            }

            var userResponse = new Model.UserResponse(saveResult.Get());
            return Result<UserResponse, string>.From(userResponse, null);
        }

        public async Task<Result<UserResponse, string>> GetProfile(string id)
        {
            var findByIdResult = await _userRepository.FindById(id);
            if (findByIdResult.IsError())
            {
                return Result<UserResponse, string>.From(null, findByIdResult.Error());
            }

            var userResponse = new Model.UserResponse(findByIdResult.Get());
            return Result<UserResponse, string>.From(userResponse, null);
        }

        public async Task<Result<List<UserResponse>, string>> GetUsers(UserFilter userFilter)
        {
            var findAllResult = await _userRepository.FindAll(userFilter);
            if (findAllResult.IsError())
            {
                return Result<List<UserResponse>, string>.From(null, findAllResult.Error());
            }

            var userResponses = new List<UserResponse>();
            findAllResult.Get().ForEach(user => {
                var userResponse = new UserResponse(user);
                userResponses.Add(userResponse);
            });

            return Result<List<UserResponse>, string>.From(userResponses, null);
        }

        public async Task<Result<UserResponse, string>> Login(LoginRequest loginRequest)
        {
            var findByUsernameResult = await _userRepository.FindByUsername(loginRequest.Username);
            if (findByUsernameResult.IsError())
            {
                return Result<UserResponse, string>.From(null, findByUsernameResult.Error());
            }

            if (findByUsernameResult.IsEmpty())
            {
                return Result<UserResponse, string>.From(null, "invalid username or password");
            }

            var user = findByUsernameResult.Get();

            if (!user.IsValidPassword(loginRequest.Password))
            {
                return Result<UserResponse, string>.From(null, "invalid username or password");
            }

            return Result<UserResponse, string>.From(new UserResponse(user), null);
        }
    }
}