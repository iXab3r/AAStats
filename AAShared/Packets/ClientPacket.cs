using System.IO;
using System.Text;

using AAShared.BinaryClasses;
using AAShared.Packets.Enums;

using XMLib;

namespace AAShared.Packets
{
    public class ClientPacket : BasicPacket
    {
        protected ClientOpCode m_opCode;

        public ClientOpCode OpCode
        {
            get
            {
                return m_opCode;
            }
        }

        public ClientPacket(ClientOpCode _opCode)
        {
            m_opCode = _opCode;
        }

        public ClientPacket()
            : this(ClientOpCode.X2EnterWorldPacket)
        {

        }

        public static ClientPacket Create(byte[] _inputData)
        {
            var result = new ClientPacket();
            result.Parse(_inputData);
            return result;
        }

        /// <summary>
        ///   Метод, который вызывается, когда идет запрос на парсинг пакета с использованием указанного диссектора
        /// </summary>
        /// <param name="_dissector"></param>
        protected override void InternalParse(PacketDissector _dissector)
        {
            base.InternalParse(_dissector);
            var expectedOpCode = m_opCode;
            m_opCode = _dissector.ReadValue<ClientOpCode>();
            if (expectedOpCode != ClientOpCode.X2EnterWorldPacket && m_opCode != expectedOpCode)
            {
                throw new InvalidDataException(string.Format("Expected packet type {0}, got {1}", expectedOpCode, m_opCode));
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return FormatHeader();
        }

        protected string FormatHeader(bool _dumpHex = true)
        {
            var result = new StringBuilder();
            result.AppendFormat("{0} ClientPacket {1}(0x{2:X4})", Header, OpCode, (ushort)OpCode);
            if (_dumpHex)
            {
                result.AppendFormat("\r\n{0}", StringUtils.HexDump(Data));
            }
            return result.ToString();
        }

    }
}