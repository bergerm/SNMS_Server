﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SNMS_Server.Variables;

namespace SNMS_Server.RealTimeEngine
{
    class CatenateStringCommand : StringCommand
    {
        public CatenateStringCommand(string destination,
                                        string source1,
                                        bool s1Variable,
                                        string source2,
                                        bool s2Variable) :
            base("Catenate", destination, source1, s1Variable, source2, s2Variable)
        {
        }

        protected override bool CommandLogic()
        {
            string temp = GetSource1() + GetSource2();
            StringVariable newVar = new StringVariable(temp);
            m_variableDictionary.SetVariable(m_sDestination, newVar);

            return true;
        }
    }
}
