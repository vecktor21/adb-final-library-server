﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Dal.Options
{
    public class ConnectionOptions 
    {
        public string ConnectionsString { get; set; } = null!;
        public string Database { get; set; } = null!;
        public string UserCollectionName { get; set; } = null!;
        public string BookCollectionName { get; set; } = null!;
        public string CartCollectionName { get; set; } = null!;
    }
}
