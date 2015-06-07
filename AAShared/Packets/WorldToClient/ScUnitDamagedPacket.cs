using System;

using AAShared.BinaryClasses;
using AAShared.Packets.Enums;

namespace AAShared.Packets
{
    public class ScUnitDamagedPacket : WorldPacket
    {
        public uint TargetId { get; set; }

        public uint SourceId { get; set; }


        public ushort SkillId { get; set; }

        public uint Damage { get; set; }
        public uint Absorb { get; set; }

        public DamagePacketType DmgType { get; set; }


        public ScUnitDamagedPacket() : base(WorldOpCode.UnitDamagedPacket)
        {
            
        }

        /// <summary>
        ///   Метод, который вызывается, когда идет запрос на парсинг пакета с использованием указанного диссектора
        /// </summary>
        /// <param name="_dissector"></param>
        protected override void InternalParse(PacketDissector _dissector)
        {
            base.InternalParse(_dissector);

            /*
                FSkip(11);
                ushort skillId;
                FSkip(3);
                AAId casterId;
                AAId playerId;  
                AAId targetId;
                FSkip(2);
                ushort dmg;
                ushort absorb;
             */
            DmgType = _dissector.ReadValue<DamagePacketType>();
            switch (DmgType)
            {
                case DamagePacketType.SimpleCast:
                    _dissector.Skip(4+2);
                    break;
                case DamagePacketType.SimpleWithEffect:
                    _dissector.Skip(14);
                    break;
                case DamagePacketType.Type2:
                    _dissector.Skip(4);
                    TargetId = _dissector.ReadId();
                    _dissector.Skip(6);
                    break;
                case DamagePacketType.Type3:
                    _dissector.Skip(8);
                    TargetId = _dissector.ReadId();
                    break;
                case DamagePacketType.Type4:
                    _dissector.Skip(7);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            var subType = _dissector.ReadValue<DamagePacketSubType>();
            SourceId = _dissector.ReadId();
            switch (subType)
            {
                case DamagePacketSubType.Type0:
                    break;
                case DamagePacketSubType.Type1:
                    break;
                case DamagePacketSubType.Type2:
                    _dissector.Skip(20);
                    break;
                case DamagePacketSubType.Type3:
                    _dissector.Skip(4);
                    break;
                case DamagePacketSubType.Type4:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            TargetId = _dissector.ReadId();
            _dissector.ReadId();

            _dissector.Skip(1);
            var dmgByte = _dissector.ReadValue<byte>();

            Damage = ((int)dmgByte & 2) == 0 ? (((int)dmgByte & 1) == 0 ? (uint)_dissector.ReadValue<byte>() : (uint)_dissector.ReadValue<ushort>()) : _dissector.ReadId();
            Absorb = ((int)dmgByte & 8) == 0 ? (((int)dmgByte & 4) == 0 ? (uint)_dissector.ReadValue<byte>() : (uint)_dissector.ReadValue<ushort>()) : _dissector.ReadId();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return String.Format(
                "{0}\r\n{1}",
                this.FormatHeader(true),
                String.Format(
                    "[{5}] Unit 0x{0:X6} dealt {1}(+{2}) dmg to target 0x{3:X6} using skill 0x{4:X6}",
                    this.SourceId,
                    this.Damage,
                    this.Absorb,
                    this.TargetId,
                    this.SkillId,
                    this.DmgType));
        }
    }

    public enum DamagePacketType :byte
    {
        SimpleCast,
        SimpleWithEffect,
        Type2,
        Type3,
        Type4
    }

    public enum DamagePacketSubType : byte
    {
        Type0,
        Type1,
        Type2,
        Type3,
        Type4
    }
}