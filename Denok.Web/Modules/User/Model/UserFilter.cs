using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Denok.Web.Modules.User.Model
{
    public class UserFilter : Base.Filter.BaseFilter
    {
        public UserFilter()
        {
            Page = 1;
            Limit = 10;
            Sort = "ASC";
        }
    }
}