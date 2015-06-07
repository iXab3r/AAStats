using System;

using AAShared.BinaryClasses;
using AAShared.Packets.Enums;

namespace AAShared.Packets.ClientToWorld
{
    public class CsTargetChangedPacket : WorldPacket
    {
        public uint CharId { get; set; }

        public uint TargetId { get; set; }

        public CsTargetChangedPacket() : base(WorldOpCode.TargetChangedPacket)
        {
            
        }

        /// <summary>
        ///   Метод, который вызывается, когда идет запрос на парсинг пакета с использованием указанного диссектора
        /// </summary>
        /// <param name="_dissector"></param>
        protected override void InternalParse(PacketDissector _dissector)
        {
            base.InternalParse(_dissector);

            CharId = _dissector.ReadId();
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
            return String.Format("{0}\r\n\t 0x{1:X6} > 0x{2:X6}", this.FormatHeader(false), this.CharId, this.TargetId);
        }
    }
}