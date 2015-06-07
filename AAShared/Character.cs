using System.Collections.Concurrent;
using System.Collections.Generic;

namespace AAShared
{
    public class Character
    {
        public uint Id { get; private set; }

        public string Name;

        public IEnumerable<CharacterBuff> Buffs
        {
            get
            {
                return m_buffs.Values;
            }
        }

        private ConcurrentDictionary<uint, CharacterBuff> m_buffs = new ConcurrentDictionary<uint, CharacterBuff>();

        public Character(uint _charId)
        {
            Id = _charId;
        }

        public void AddBuff(CharacterBuff _characterBuff)
        {
            m_buffs.TryAdd(_characterBuff.Index, _characterBuff);
        }

        public CharacterBuff RemoveBuff(uint _buffIndex)
        {
            CharacterBuff trash;
            m_buffs.TryRemove(_buffIndex, out trash);
            return trash;
        }
    }
}
