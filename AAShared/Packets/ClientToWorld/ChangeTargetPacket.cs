using System;

using AAShared.BinaryClasses;
using AAShared.Packets.Enums;

namespace AAShared.Packets.ClientToWorld
{
    class ChangeTargetPacket : ClientPacket
    {
        public uint TargetId { get; set; }

        public ChangeTargetPacket() : base(ClientOpCode.ChangeTargetPacket)
        {
            
        }

        /// <summary>
        ///   Метод, который вызывается, когда идет запрос на парсинг пакета с использованием указанного диссектора
        /// </summary>
        /// <param name="_dissector"></param>
        protected override void InternalParse(PacketDissector _dissector)
        {
            base.InternalParse(_dissector);
            TargetId = _dissector.ReadId();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return String.Format("{0}\r\n\t Player > 0x{1:X6} ", this.FormatHeader(false), this.TargetId);
        }
    }
}