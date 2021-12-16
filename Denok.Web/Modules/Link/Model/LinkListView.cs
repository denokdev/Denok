using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Denok.Web.Modules.Link.Model
{

    public class LinkListView
    {
        public Lib.Shared.Meta Meta { get; set; }
        public List<Model.LinkResponse> LinkData { get; set; }
        
    }
}