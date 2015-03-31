using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMS_Server.RealTimeEngines.Sequences
{
    class SequenceTimer
    {
        int m_dwPeriod;
        string m_sSequenceName;

        public int GetPeriod() { return m_dwPeriod; }
        public void SetPeriod(int period) { m_dwPeriod = period; }

        public string GetSequenceName() { return m_sSequenceName; }
        public void SetSequenceName(string name) { m_sSequenceName = name; }
    }
}
