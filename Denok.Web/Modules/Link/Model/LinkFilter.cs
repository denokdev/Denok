using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Denok.Web.Modules.Link.Model
{
    public class LinkFilter : Base.Filter.BaseFilter
    {
        public LinkFilter()
        {
            Page = 1;
            Limit = 5;
            Sort = "DESC";
        }
    }
}