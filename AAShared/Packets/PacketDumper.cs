using System;
using System.IO;
using System.Threading;

namespace AAShared.Packets
{
    public static class PacketDumper
    {
        private static int packetIndex = 0;

        private static string m_packetsPath = "";

        static PacketDumper()
        {
            m_packetsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "packets");
            if (Directory.Exists(m_packetsPath))
            {
                Directory.Delete(m_packetsPath,true);
            }
        }

        public static void Dump(byte[] _packet)
        {
            if( _packet == null)
            {
                return;
            }
            Interlocked.Increment(ref packetIndex);
            if (!Directory.Exists(m_packetsPath))
            {
                Directory.CreateDirectory(m_packetsPath);
            }

            var packetFileName = "packet " + packetIndex;
            File.WriteAllBytes(Path.Combine(m_packetsPath, packetFileName), _packet);
        }

    }
}
