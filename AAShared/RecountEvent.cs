using System;

using RecoundLib;

namespace AAShared
{
    public class RecountEvent
    {
        private uint m_id;

        public uint Id
        {
            get
            {
                return m_id;
            }
        }

        public RecountEvent()
        {
            m_id = IdGenerator.Instance.GetNextId();
        }

        public DateTime Timestamp;

        public uint SourceId;

        public uint TargetId;
    }
}