using System;
using System.Collections.Concurrent;
using System.Threading;

using XMLib;
using XMLib.Statistics;

namespace AAShared
{
    public class RawPacketsRetranslator
    {
	    private readonly DataSupplier m_packetsSummator;

	    private Thread m_thread;
	    private ManualResetEvent m_threadCancelEvent;

		public event EventHandler<EventArgs<byte[]>> RawPacketReceived = delegate { };
        public event EventHandler<EventArgs<byte[]>> RawPacketSent = delegate { };

        private BlockingCollection<PacketRecord> m_packets;

        public RawPacketsRetranslator(DataSupplier _packetsSummator)
        {
	        m_packetsSummator = _packetsSummator;
	        m_packets=  new BlockingCollection<PacketRecord>();
            _packetsSummator.RawPacketReceived += (_sender, _args) => m_packets.Add(new PacketRecord(_args.Value, PacketType.Incoming));
            _packetsSummator.RawPacketSent += (_sender, _args) => m_packets.Add(new PacketRecord(_args.Value, PacketType.Outgoing));
            m_thread = new Thread(WorkerMethod){IsBackground =  true,  Name = "Pckts"};
			m_threadCancelEvent = new ManualResetEvent(false);
            m_thread.Start();
        }

        private void WorkerMethod()
        {
            do
            {
                PacketRecord packetRecord;
                while (m_packets.TryTake(out packetRecord))
                {
                    switch (packetRecord.PType)
                    {
                        case PacketType.Incoming:
                            RawPacketReceived(this, new EventArgs<byte[]>(packetRecord.Packet));
                            break;
                        case PacketType.Outgoing:
                            RawPacketSent(this, new EventArgs<byte[]>(packetRecord.Packet));
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            } while (!m_threadCancelEvent.WaitOne(0));
        }

        private struct PacketRecord
        {
            public PacketType PType;

            public byte[] Packet;

            public PacketRecord(byte[] _packet, PacketType _pType)
                : this()
            {
                Packet = _packet;
                PType = _pType;
            }
        }

        private enum PacketType
        {
            Unknown,
            Incoming,
            Outgoing,
        }
    }
}
