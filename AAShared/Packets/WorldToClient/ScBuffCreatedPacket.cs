using System;

using AAShared.BinaryClasses;
using AAShared.Packets.Enums;

namespace AAShared.Packets.WorldToClient
{
    public class ScBuffCreatedPacket : WorldPacket
    {
        public uint CharId { get; set; }
        public uint TargetId { get; set; }

        public uint BuffIndex { get; set; }

        public uint BuffId { get; set; }

        public ScBuffCreatedPacket() : base(WorldOpCode.SCBuffCreatedPacket)
        {
            
        }

        /// <summary>
        ///   Метод, который вызывается, когда идет запрос на парсинг пакета с использованием указанного диссектора
        /// </summary>
        /// <param name="_dissector"></param>
        protected override void InternalParse(PacketDissector _dissector)
        {
            base.InternalParse(_dissector);
            _dissector.ReadArray<byte>(1); // ID ?
            CharId = _dissector.ReadId();

            var someId = _dissector.ReadValue<uint>();

            TargetId = _dissector.ReadId();
            BuffIndex = _dissector.ReadValue<uint>();
            BuffId = _dissector.ReadValue<uint>();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return String.Format("{0}\r\n\t{1:X6} > {2:X6}, buff {3:X8} (index {4:X8})",
                this.FormatHeader(false), this.CharId, this.TargetId, this.BuffId, this.BuffIndex);
        }
    }
}