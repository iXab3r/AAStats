using System;

using AAShared.BinaryClasses;
using AAShared.Packets.Enums;

namespace AAShared.Packets.WorldToClient
{
    class ScBuffRemovedPacket : WorldPacket
    {

        public uint CharId { get; set; }
        public uint BuffIndex { get; set; }

        public ScBuffRemovedPacket() : base(WorldOpCode.SCBuffRemovedPacket)
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
            BuffIndex = _dissector.ReadValue<uint>();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return String.Format("{0}\r\n\tRemove buff {1:X6} from {2:X6}", this.FormatHeader(false), this.BuffIndex, this.CharId);
        }
    }
}