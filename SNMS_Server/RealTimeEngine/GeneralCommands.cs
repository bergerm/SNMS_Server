﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMS_Server.RealTimeEngine
{
    abstract class GeneralCommand : Command
    {
        public GeneralCommand(string subType)
            : base("General", subType)
        {

        }
    }
}
