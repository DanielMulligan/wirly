using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Wirly.web.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [AllowHtml]
        public string Body { get; set; }
        public virtual ICollection<AppUser> Users { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<DocumentType> DocumentTypes { get; set; }

        public Project()
        {
                   
        }
    }
}