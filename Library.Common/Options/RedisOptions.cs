﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Common.Options
{
    public class RedisOptions
    {
        public int CacheLifeTimeMinutes { get; set; }
        public int ConnectionTimeout { get; set; }
        public int SlidingExpiration { get; set; }
    }
}