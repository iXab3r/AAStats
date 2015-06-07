using System;

using AAShared.BinaryClasses;

namespace AAShared.Packets.Enums
{
    public class MessageHeader : BinaryPacket
    {
        public ushort MessageLength;

        public byte UNC;

        public SubType PacketType;

        public const ushort Length = 4;

        /// <summary>
        ///   Метод, который вызывается, когда идет запрос на парсинг пакета с использованием указанного диссектора
        /// </summary>
        /// <param name="_dissector"></param>
        protected override void InternalParse(PacketDissector _dissector)
        {
            base.InternalParse(_dissector);
            MessageLength = _dissector.ReadValue<ushort>();
            UNC = _dissector.ReadValue<byte>();
            PacketType = _dissector.ReadValue<SubType>();
        }

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing a fully qualified type name.
        /// </returns>
        public override string ToString()
        {
            return String.Format("Hdr {0}b UNC 0x{1:X} PacketType {2}(0x{3:X2})", MessageLength, UNC, PacketType, (byte)PacketType);

        }
    }
}