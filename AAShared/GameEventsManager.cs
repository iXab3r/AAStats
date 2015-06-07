using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace AAShared
{
    public class GameEventsManager : IEnumerable<RecountEvent>
    {
        private ConcurrentDictionary<uint, RecountEvent> m_events = new ConcurrentDictionary<uint, RecountEvent>();

        private ConcurrentDictionary<uint, ConcurrentQueue<RecountEvent>> m_eventsByCharId = new ConcurrentDictionary<uint, ConcurrentQueue<RecountEvent>>();

        public GameEventsManager()
        {

        }

        public void AddEvent(RecountEvent _event)
        {
            m_events.TryAdd(_event.Id, _event);

            var eventsList = GetOrCreateCharEventsList(_event.SourceId);
            eventsList.Enqueue(_event);
        }

        public IEnumerable<RecountEvent> GetEventsByCharId(uint _charId)
        {
            return GetOrCreateCharEventsList(_charId);
        }

        public void Clear()
        {
            m_eventsByCharId.Clear();
            m_events.Clear();
        }

        private ConcurrentQueue<RecountEvent> GetOrCreateCharEventsList(uint _charId)
        {
            ConcurrentQueue<RecountEvent> eventsList;
            if (!m_eventsByCharId.TryGetValue(_charId, out eventsList))
            {
                eventsList = new ConcurrentQueue<RecountEvent>();
                m_eventsByCharId[_charId] = eventsList;
            }
            return eventsList;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<RecountEvent> GetEnumerator()
        {
            return m_events.Values.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}