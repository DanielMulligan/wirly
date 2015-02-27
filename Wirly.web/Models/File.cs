using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wirly.web.Models
{
    public class File
    {
        public int Id { get; set; }
        public string  Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public int Version { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
    }
}