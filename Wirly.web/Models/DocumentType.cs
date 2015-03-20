using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wirly.web.Models
{
    public class DocumentType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project {get; set;}
    }
}