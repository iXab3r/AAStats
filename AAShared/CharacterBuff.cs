using System;
using System.ComponentModel;

using AAShared.Database;

using XMLib.Log;

namespace AAShared
{
    public class CharacterBuff
    {
        private BuffInfo m_info;

        public uint Id { get; set; }

        [Browsable(false)]
        public uint Index { get; set; }

        [Browsable(false)]
        public uint CharId { get; set; }

        public BuffInfo Info
        {
            get
            {
                return m_info;
            }
        }

        public string Name
        {
            get
            {
                return m_info != null ? m_info.Name : "Unknown";
            }
        }

        [Browsable(false)]
        public TimeSpan Duration { get; set; }

        public CharacterBuff(uint _charId, uint _buffId, uint _index)
        {
            CharId = _charId;
            Id = _buffId;
            Index = _index;
            if (!AADb.Instance.TryToGetBuff(_buffId, out m_info))
            {
                Logger.WarnFormat("[CharacterBuff..ctor] Unknown buff {0:X8}",_buffId);
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            if (m_info == null)
            {
                return String.Format("CharacterBuff {0:X8}", Id);
            } else
            {
                return String.Format("{0}", m_info.Name);
            }
        }
    }
}