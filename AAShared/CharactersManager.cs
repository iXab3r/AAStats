using System;
using System.Collections.Concurrent;
using System.Threading;

using XMLib;

namespace AAShared
{
    public class CharactersManager
    {
        private ConcurrentDictionary<uint, Character> m_characters = new ConcurrentDictionary<uint, Character>();

        public event EventHandler<BuffUpdatedEventArgs> BuffUpdated = delegate { };

        private Thread m_buffsThread;

	    private ManualResetEvent m_buffsThreadCancelEvent;

		public CharactersManager(PacketsProcessor _packetsProcessor)
        {
            m_buffsThread = new Thread(WorkerMethod) { Name = "BuffsChecker", IsBackground = true };
			m_buffsThreadCancelEvent = new ManualResetEvent(false);
            m_buffsThread.Start();
            _packetsProcessor.BuffCreated += PacketsProcessorOnBuffCreated;
            _packetsProcessor.BuffRemoved += PacketsProcessorOnBuffRemoved;
        }

        private void WorkerMethod()
        {
            var lastCheckTimestamp = DateTime.MinValue;
            do
            {
                var now = DateTime.Now;
                try
                {
                    if (lastCheckTimestamp != DateTime.MinValue)
                    {
                        var timeElapsed = now - lastCheckTimestamp;
                        foreach (var kvp in m_characters)
                        {
                            var character = kvp.Value;
                            foreach (var buff in character.Buffs)
                            {
                                buff.Duration -= timeElapsed;
                                if (buff.Duration.Ticks < 0)
                                {
                                    buff.Duration = TimeSpan.FromSeconds(0);
                                }
                            }
                        }
                    }
                }
                finally
                {
                    lastCheckTimestamp = now;
                }

            } while (!m_buffsThreadCancelEvent.WaitOne(100));
        }

        private void PacketsProcessorOnBuffRemoved(object _sender, BuffRemovedEventArgs _buffRemovedEventArgs)
        {
            var character = this.GetOrCreate(_buffRemovedEventArgs.CharId);
            var buff = character.RemoveBuff(_buffRemovedEventArgs.BuffIndex);
            if (buff != null)
            {
                BuffUpdated(this, new BuffUpdatedEventArgs(buff));
            }
        }


        private void PacketsProcessorOnBuffCreated(object _sender, BuffCreatedEventArgs _buffCreatedEventArgs)
        {
            var character = this.GetOrCreate(_buffCreatedEventArgs.TargetId);
            character.AddBuff(_buffCreatedEventArgs.NewCharacterBuff);
            BuffUpdated(this, new BuffUpdatedEventArgs(_buffCreatedEventArgs.NewCharacterBuff));
        }

        public Character GetOrCreate(uint _charId)
        {
            Character @char;
        @retry:
            if (!m_characters.TryGetValue(_charId, out @char))
            {
                @char = new Character(_charId);
                m_characters.TryAdd(_charId, @char);
                goto @retry;
            }
            return @char;
        }
    }

    public class BuffUpdatedEventArgs : EventArgs
    {
        public CharacterBuff Buff { get; private set; }

        public BuffUpdatedEventArgs(CharacterBuff _charBuff)
        {
	        if (_charBuff == null)
	        {
		        throw new ArgumentNullException(nameof(_charBuff));
	        }
	        Buff = _charBuff;
        }
    }
}