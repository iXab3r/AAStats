using System.Threading;

using XMLib;

namespace RecoundLib
{
    internal class IdGenerator : LazySingleton<IdGenerator>
    {
        private long m_id = 0;
        public uint GetNextId()
        {
            return (uint)Interlocked.Increment(ref m_id);
        }
    }
}