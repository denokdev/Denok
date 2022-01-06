using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Bson;
using Denok.Lib.Shared;
using Denok.Lib.Database.Mongo;
using Denok.Web.Modules.Link.Model;

namespace Denok.Web.Modules.Link.Repository
{
    public class LinkRepositoryMongo : ILinkRepository
    {
        private readonly ILogger<LinkRepositoryMongo> _logger;
        private readonly IMongoCollection<Model.Link> _linkCollection;

        public LinkRepositoryMongo(ILogger<LinkRepositoryMongo> logger, IMongo mongo)
        {
            _logger = logger;
            _linkCollection = mongo.Database().GetCollection<Model.Link>("links");
        }

        public async Task<Result<long, string>> Count(LinkFilter linkFilter)
        {
            try 
            {
                var count = await _linkCollection.CountDocumentsAsync(_ => true);
                return Result<long, string>.From(count, null);
            } catch(Exception e)
            {
                return Result<long, string>.From(0, e.ToString());
            }
        }

        public async Task<Result<List<Model.Link>, string>> FindAll(LinkFilter linkFilter)
        {
            linkFilter.CalculateOffset();

            // var filter = new BsonDocument(){{"a", "b"}, {"c", "d"}};
            try 
            {
                var findFluent = _linkCollection.Find<Model.Link>(_ => true);
                findFluent.Skip((int) linkFilter.Offset);
                findFluent.Limit((int) linkFilter.Limit);
                
                if (!String.IsNullOrEmpty(linkFilter.OrderBy))
                {
                    if (linkFilter.Sort.ToUpper() == "ASC")
                    {
                        var sortDefinition = Builders<Model.Link>.Sort.Ascending(_ => linkFilter.OrderBy);
                        findFluent.Sort(sortDefinition);
                    } else
                    {
                        var sortDefinition = Builders<Model.Link>.Sort.Descending(_ => linkFilter.OrderBy);
                        findFluent.Sort(sortDefinition);
                    }
                } else
                {
                    if (linkFilter.Sort.ToUpper() == "ASC")
                    {
                        var sortDefinition = Builders<Model.Link>.Sort.Ascending(x => x.CreatedAt);
                        findFluent.Sort(sortDefinition);
                    } else
                    {
                        var sortDefinition = Builders<Model.Link>.Sort.Descending(x => x.CreatedAt);
                        findFluent.Sort(sortDefinition);
                    }
                }

                var data = await findFluent.ToListAsync();

                return Result<List<Model.Link>, string>.From(data, null);
            } catch(Exception e)
            {
                return Result<List<Model.Link>, string>.From(null, e.ToString());
            }
        }

        public async Task<Result<Model.Link, string>> FindByGeneratedLink(string generatedLink)
        {
            try 
            {
                var link = await _linkCollection
                    .Find(x => x.GeneratedLink.ToUpper() == generatedLink.ToUpper())
                    .FirstAsync();

                return Result<Model.Link, string>.From(link, null);
            } catch(Exception e)
            {
                return Result<Model.Link, string>.From(null, e.ToString());
            }
        }

        public async Task<Result<Model.Link, string>> FindById(string id)
        {
            try 
            {
                var link = await _linkCollection
                    .Find(x => x.Id == id)
                    .FirstAsync();

                return Result<Model.Link, string>.From(link, null);
            } catch(Exception e)
            {
                return Result<Model.Link, string>.From(null, e.ToString());
            }
        }

        public async Task<Result<Model.Link, string>> Save(Model.Link link)
        {
            try 
            {
                await _linkCollection.InsertOneAsync(link);
                return Result<Model.Link, string>.From(link, null);
            } catch(Exception e)
            {
                return Result<Model.Link, string>.From(null, e.ToString());
            }
        }

        public async Task<Result<Model.Link, string>> Update(string generatedLink, Model.Link link)
        {
            try 
            {
                await _linkCollection.ReplaceOneAsync(
                    x => x.GeneratedLink.ToUpper() == generatedLink.ToUpper(), link);
                return Result<Model.Link, string>.From(link, null);
            } catch(Exception e)
            {
                return Result<Model.Link, string>.From(null, e.ToString());
            }
        }

        public async Task<Result<long, string>> Remove(string id)
        {
            try 
            {   
                var deleteResult = await _linkCollection.DeleteOneAsync(x => x.Id == id);
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