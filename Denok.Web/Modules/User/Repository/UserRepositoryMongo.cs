using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Bson;
using Denok.Lib.Shared;
using Denok.Lib.Database.Mongo;
using Denok.Web.Modules.User.Model;

namespace Denok.Web.Modules.User.Repository
{
    public class UserRepositoryMongo : IUserRepository
    {
        private readonly ILogger<UserRepositoryMongo> _logger;
        private readonly IMongoCollection<Model.User> _userCollection;

        public UserRepositoryMongo(ILogger<UserRepositoryMongo> logger, IMongo mongo)
        {
            _logger = logger;
            _userCollection = mongo.Database().GetCollection<Model.User>("users");
        }

        public async Task<Result<long, string>> Count(UserFilter userFilter)
        {
            try 
            {
                var count = await _userCollection.CountDocumentsAsync(_ => true);
                return Result<long, string>.From(count, null);
            } catch(Exception e)
            {
                return Result<long, string>.From(0, e.ToString());
            }
        }

        public async Task<Result<List<Model.User>, string>> FindAll(UserFilter userFilter)
        {
            userFilter.CalculateOffset();

            // var filter = new BsonDocument(){{"a", "b"}, {"c", "d"}};
            try 
            {
                var findFluent = _userCollection.Find<Model.User>(_ => true);
                findFluent.Skip((int) userFilter.Offset);
                findFluent.Limit((int) userFilter.Limit);
                
                if (!String.IsNullOrEmpty(userFilter.OrderBy))
                {
                    if (userFilter.Sort.ToUpper() == "ASC")
                    {
                        var sortDefinition = Builders<Model.User>.Sort.Ascending(_ => userFilter.OrderBy);
                        findFluent.Sort(sortDefinition);
                    } else
                    {
                        var sortDefinition = Builders<Model.User>.Sort.Descending(_ => userFilter.OrderBy);
                        findFluent.Sort(sortDefinition);
                    }
                } else
                {
                    if (userFilter.Sort.ToUpper() == "ASC")
                    {
                        var sortDefinition = Builders<Model.User>.Sort.Ascending(x => x.CreatedAt);
                        findFluent.Sort(sortDefinition);
                    } else
                    {
                        var sortDefinition = Builders<Model.User>.Sort.Descending(x => x.CreatedAt);
                        findFluent.Sort(sortDefinition);
                    }
                }

                var data = await findFluent.ToListAsync();

                return Result<List<Model.User>, string>.From(data, null);
            } catch(Exception e)
            {
                return Result<List<Model.User>, string>.From(null, e.ToString());
            }
        }

        public async Task<Result<Model.User, string>> FindByEmail(string email)
        {
            try 
            {
                var user = await _userCollection
                    .Find(x => x.Email.ToUpper() == email.ToUpper())
                    .FirstAsync();

                return Result<Model.User, string>.From(user, null);
            } catch(Exception e)
            {
                return Result<Model.User, string>.From(null, e.ToString());
            }
        }

        public async Task<Result<Model.User, string>> FindById(string id)
        {
            try 
            {
                var user = await _userCollection
                    .Find(x => x.Id == id)
                    .FirstAsync();

                return Result<Model.User, string>.From(user, null);
            } catch(Exception e)
            {
                return Result<Model.User, string>.From(null, e.ToString());
            }
        }

        public async Task<Result<Model.User, string>> FindByUsername(string username)
        {
            try 
            {
                var user = await _userCollection
                    .Find(x => x.Username.ToUpper() == username.ToUpper())
                    .FirstAsync();

                return Result<Model.User, string>.From(user, null);
            } catch(Exception e)
            {
                return Result<Model.User, string>.From(null, e.ToString());
            }
        }

        public async Task<Result<Model.User, string>> Save(Model.User user)
        {
            try 
            {
                await _userCollection.InsertOneAsync(user);
                return Result<Model.User, string>.From(user, null);
            } catch(Exception e)
            {
                return Result<Model.User, string>.From(null, e.ToString());
            }
        }

        public async Task<Result<long, string>> Remove(string id)
        {
            try 
            {   
                var deleteResult = await _userCollection.DeleteOneAsync(x => x.Id == id);
                if (deleteResult.DeletedCount <= 0)
                {
                    return Result<long, string>.From(0, "document not found");
                }

                return Result<long, string>.From(deleteResult.DeletedCount, null);
            } catch(Exception e)
            {
                return Result<long, string>.From(0, e.ToString());
            }
        }
    }
}