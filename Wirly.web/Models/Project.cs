﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wirly.web.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<AppUser> Users { get; set; }
    }
}