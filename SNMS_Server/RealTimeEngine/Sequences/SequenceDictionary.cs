using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMS_Server.RealTimeEngines.Sequences
{
    class SequenceDictionary
    {
        Dictionary<string, Sequence> m_dictionary;

        public SequenceDictionary()
        {
            m_dictionary = new Dictionary<string, Sequence>();
        }

        public void Clear()
        {
            m_dictionary = new Dictionary<string, Sequence>();
        }

        public bool SequenceExists(string sequenceName)
        {
            return m_dictionary.ContainsKey(sequenceName);
        }

        public void SetSequence(string sequenceName, Sequence sequence)
        {
            m_dictionary[sequenceName] = sequence;
        }

        public Sequence GetSequence(string sequenceName)
        {
            if (!SequenceExists(sequenceName))
            {
                return null;
            }

            return m_dictionary[sequenceName];
        }
    }
}
