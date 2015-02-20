using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wirly.web.Models
{
    public class ProjectEditModel
    {
        public Project Project { get; set; }
        public IEnumerable<UserAssignModel> Members { get; set; }
        public IEnumerable<UserAssignModel> NonMembers { get; set; }
    }
}