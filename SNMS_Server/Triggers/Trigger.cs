using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SNMS_Server.Variables;
using SNMS_Server.RealTimeEngines.Sequences;

namespace SNMS_Server.Triggers
{
    class Trigger
    {
        int m_dwTriggerId;
        string m_sValue;
        Sequence m_reaction;
        string m_sReactionValue;
        bool m_bEnabled;

        public void SetID(int id) { m_dwTriggerId = id; }
        public int GetID() { return m_dwTriggerId; }

        public void SetValue(string sValue) { m_sValue = sValue; }
        public string GetValue() { return m_sValue; }

        public void SetReaction(Sequence reaction) { m_reaction = reaction; }
        public Sequence GetReaction() { return m_reaction; }

        public void SetReactionValue(string sReactionValue) { m_sReactionValue = sReactionValue; }
        public string GetReactionValue() { return m_sReactionValue; }

        public void SetEnabled(bool isEnabled) { m_bEnabled = isEnabled; }
        public bool GetEnabled() { return m_bEnabled; }

        public Trigger Clone()
        {
            Trigger newTrigger = new Trigger();

            newTrigger.SetID(GetID());
            newTrigger.SetValue(GetValue());
            newTrigger.SetReaction(GetReaction());
            newTrigger.SetReactionValue(GetReactionValue());
            newTrigger.SetEnabled(GetEnabled());

            return newTrigger;
        }

        public bool Validate(string sStringToValidate)
        {
            if (sStringToValidate.Contains(m_sValue))
            {
                return true;
            }

            return false;
        }
    }
}
