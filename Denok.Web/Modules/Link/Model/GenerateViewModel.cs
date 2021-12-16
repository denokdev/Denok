using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Denok.Web.Modules.Link.Model
{

    public class GenerateViewModel
    {
        public string Username { get; set; }

        public Modules.User.Model.UserResponse Profile { get; set; }

        public GenerateRequest GenerateRequest { get; set; }

        public string GeneratedLink { get; set; }

        public string ErrorMessage { get; set; }

        public LinkListView LinkListView { get; set; }
        
    }
}