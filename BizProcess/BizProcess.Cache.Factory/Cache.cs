using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BizProcess.Cache.Factory
{
    public class Cache
    {
        public static Interface.ICache CreateInstance()
        {
            return new BizProcess.Cache.InProc.Cache();
        }
    }
}
