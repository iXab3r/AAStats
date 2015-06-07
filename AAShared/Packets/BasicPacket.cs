using System;

using AAShared.BinaryClasses;
using AAShared.Packets.Enums;

using XMLib;

namespace AAShared.Packets
{
    public class BasicPacket : BinaryPacket
    {
        public MessageHeader Header;

        private byte[] m_data;

        public byte[] Data {
            get
            {
                return m_data;
            }
            protected set
            {
                m_data = value;
            }
        }

        public BasicPacket()
        {
            Header = new MessageHeader();
            m_data = new byte[0];
        }

        /// <summary>
        ///   Метод, который вызывается, когда идет запрос на парсинг пакета с использованием указанного диссектора
        /// </summary>
        /// <param name="_dissector"></param>
        protected override void InternalParse(PacketDissector _dissector)
        {
            base.InternalParse(_dissector);
            Header.Parse(_dissector);
            m_data = _dissector.RawData;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return String.Format("{0}\r\n{1}", Header, StringUtils.HexDump(Data,32));
        }
    }
}
