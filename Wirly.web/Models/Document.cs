using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wirly.web.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string  Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public int Version { get; set; }
        public string Html { get; set; }
        public string DocumentType { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }
    }
}