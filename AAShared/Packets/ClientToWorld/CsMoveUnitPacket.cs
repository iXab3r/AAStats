using AAShared.BinaryClasses;
using AAShared.Packets.Enums;

namespace AAShared.Packets.ClientToWorld
{
    public class CsMoveUnitPacket : ClientPacket
    {
        public uint CharId { get; set; }

        public CsMoveUnitPacket() : base(ClientOpCode.CSMoveUnitPacket)
        {
            
        }

        /// <summary>
        ///   �����, ������� ����������, ����� ���� ������ �� ������� ������ � �������������� ���������� ����������
        /// </summary>
        /// <param name="_dissector"></param>
        protected override void InternalParse(PacketDissector _dissector)
        {
            base.InternalParse(_dissector);
            CharId = _dissector.ReadId();
        }
    }
}