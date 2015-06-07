using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using AAShared.Packets;
using AAShared.Packets.Enums;

using XMLib;
using XMLib.Log;

namespace AAShared
{
    public class PacketsBuffer
    {
        public event EventHandler<EventArgs<BasicPacket>> PacketReceived = delegate { };
        public event EventHandler<EventArgs<BasicPacket>> PacketSent = delegate { };

        private List<byte> m_incomingPacketsBuffer;
        private List<byte> m_outgoingPacketsBuffer;

        private byte[] m_lastDecodedOutgoingBuffer;

        private byte[] m_lastDecodedIncomingBuffer;

        public PacketsBuffer(RawPacketsRetranslator _supplier)
        {
            m_incomingPacketsBuffer = new List<byte>();
            m_outgoingPacketsBuffer = new List<byte>();
            _supplier.RawPacketReceived += SupplierOnRawPacketReceived;
            _supplier.RawPacketSent += SupplierOnRawPacketSent;
        }

        private void SupplierOnRawPacketSent(object _sender, EventArgs<byte[]> _eventArgs)
        {
            if (_eventArgs == null || _eventArgs.Value == null || !_eventArgs.Value.Any())
            {
                return;
            }
            var data = _eventArgs.Value;
            try
            {
                ProcessOutgoingPacket(data);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("Exception occurred in event handler\r\n{0}", StringUtils.HexDump(data)), ex);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void ProcessOutgoingPacket(byte[] _packet)
        {
            var packetsCount = 0;
            byte[] packetData = null;
            try
            {
                m_outgoingPacketsBuffer.AddRange(_packet);
                MessageHeader header;
                while (PacketsFactory.Instance.TryToReadHeader(m_outgoingPacketsBuffer.ToArray(), out header))
                {
                    packetData = m_outgoingPacketsBuffer.Take(header.MessageLength + 2).ToArray();
                    try
                    {
                        //Logger.DebugFormat("Packet sent\r\n{0}", StringUtils.ToHex(packetData));
                        BasicPacket packet = PacketsFactory.Instance.Parse(packetData);
                        packetsCount++;
                        PacketSent(this, new EventArgs<BasicPacket>(packet));
                        m_lastDecodedOutgoingBuffer = packetData;
                    }
                    finally
                    {
                        m_outgoingPacketsBuffer.RemoveRange(0, packetData.Length);
                    }
                }
                if (packetsCount == 0)
                {
                    //Logger.WarnFormat("Combined packets ? \r\nBuffer state\r\n{0}\r\nNew packet\r\n{1}",StringUtils.HexDump(m_incomingPacketsBuffer.ToArray()),StringUtils.HexDump(_data));
                }
            }
            catch (Exception ex)
            {
                Logger.Error(String.Format("[Outgoing] Exception occurred during parsing buffer\r\n{2}\r\nInner: {3}\r\nBuffer state\r\n{0}\r\n\r\nPacket data:\r\n{1}\r\nLast decoded buffer:\r\n{4}",
					StringUtils.HexDump(m_incomingPacketsBuffer.ToArray()),
					packetData != null ? StringUtils.HexDump(packetData) : "<null>",
					ex.Message,
					ex.InnerException != null ? ex.InnerException.Message : "No inner exception",
					m_lastDecodedOutgoingBuffer != null ? StringUtils.HexDump(m_lastDecodedOutgoingBuffer) : "null"), ex);
            }
           
        }

        private void SupplierOnRawPacketReceived(object _sender, EventArgs<byte[]> _eventArgs)
        {
            if (_eventArgs == null || _eventArgs.Value == null || !_eventArgs.Value.Any())
            {
                return;
            }
            var data = _eventArgs.Value;
            try
            {
                this.ProcessIncomingPacket(_eventArgs.Value);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("Exception occurred in event handler\r\n{0}", StringUtils.HexDump(data)), ex);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ProcessIncomingPacket(byte[] _data)
        {
	        if (_data == null)
	        {
		        throw new ArgumentNullException(nameof(_data));
	        }
	        byte[] packetData = null;
            try
            {
                //Logger.DebugFormat("Packet arrived\r\nBytes in buffer({2}):{1}\r\n{0}", StringUtils.HexDump(_data), StringUtils.ToHex(m_incomingPacketsBuffer.ToArray()), m_incomingPacketsBuffer.Count);
                m_incomingPacketsBuffer.AddRange(_data);
                MessageHeader header;
                var packetsCount = 0;
                while (PacketsFactory.Instance.TryToReadHeader(m_incomingPacketsBuffer.ToArray(), out header))
                {
                    packetData = m_incomingPacketsBuffer.Take(header.MessageLength + 2).ToArray();
                    //Logger.DebugFormat("Packet received\r\n{0}", StringUtils.HexDump(packetData,32));
                    try
                    {
                        BasicPacket packet = PacketsFactory.Instance.Parse(packetData);
                        packetsCount++;
                        PacketReceived(this, new EventArgs<BasicPacket>(packet));
                        m_lastDecodedIncomingBuffer = packetData;
                    }
                    finally
                    {
                        m_incomingPacketsBuffer.RemoveRange(0, packetData.Length);
                    }
                }
                if (packetsCount == 0)
                {
                    //Logger.WarnFormat("Combined packets ? \r\nBuffer state\r\n{0}\r\nNew packet\r\n{1}",StringUtils.HexDump(m_incomingPacketsBuffer.ToArray()),StringUtils.HexDump(_data));
                }
            }
            catch (Exception ex)
            {
                Logger.Error(String.Format("[Incoming] Exception occurred during parsing buffer\r\nPacket data:\r\n{5}\r\n{2}\r\nInner: {3}\r\nBuffer state\r\n{0}\r\n\r\nPacket data:\r\n{1}\r\nLast decoded packet:\r\n{4}",
					StringUtils.HexDump(m_incomingPacketsBuffer.ToArray()),
					packetData != null ? StringUtils.HexDump(packetData) : "<null>",
					ex.Message,
					ex.InnerException != null ? ex.InnerException.Message : "No inner exception",
					m_lastDecodedIncomingBuffer != null ? StringUtils.HexDump(m_lastDecodedIncomingBuffer) : "null",
					StringUtils.HexDump(_data)),ex);
            }
        }

    }
}