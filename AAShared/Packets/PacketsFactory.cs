using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

using AAShared.Packets.Enums;

using XMLib;
using XMLib.Log;

namespace AAShared.Packets
{
    public class PacketsFactory
    {
        private static Lazy<PacketsFactory> m_instance = new Lazy<PacketsFactory>(true);

        private static ConcurrentDictionary<WorldOpCode, Type> m_worldPackets = new ConcurrentDictionary<WorldOpCode, Type>();
        private static ConcurrentDictionary<ClientOpCode, Type> m_clientPackets = new ConcurrentDictionary<ClientOpCode, Type>();

        private static bool RegisterPacket<T>(Type _packetType)
        {
	        if (_packetType == null)
	        {
		        throw new ArgumentNullException(nameof(_packetType));
	        }
	        if (!typeof(T).IsAssignableFrom(_packetType))
            {
                throw new ArgumentException(String.Format("Type '{0}' is not assignable from {1}", _packetType, typeof(T)));
            }
            if (_packetType.IsAbstract)
            {
                throw new ArgumentException(String.Format("Type '{0}' is abstract", _packetType));
            }
            var constructor = _packetType.GetConstructor(Type.EmptyTypes);
            if (constructor == null)
            {
                throw new ApplicationException(String.Format("Could not find parameterless constructor for type '{0}'", _packetType));
            }
            var packet = (T)Activator.CreateInstance(_packetType);
            if (typeof(T) == typeof(WorldPacket))
            {
                var worldPacket = packet as WorldPacket;
                return m_worldPackets.TryAdd(worldPacket.OpCode, _packetType);
            }
            else if (typeof(T) == typeof(ClientPacket))
            {
                var clientPacket = packet as ClientPacket;
                return m_clientPackets.TryAdd(clientPacket.OpCode, _packetType);
            }
            else
            {
                throw new ApplicationException(string.Format("Unknown packet type - {0}", typeof(T)));
            }
        }


        /// <summary>
        ///   Регистрирует все типы, которые наследуются от CtiPacket из исполняемой на данный момент сборки
        /// </summary>
        public static void RegisterPacketsFromExecutingAssembly()
        {
            var assembly = Assembly.GetExecutingAssembly();
            RegisterPacketsFromAssembly(assembly);

        }

        /// <summary>
        ///   Регистрирует все типы, которые наследуются от CtiPacket из указанной сборки
        /// </summary>
        /// <param name="_assembly"></param>
        public static void RegisterPacketsFromAssembly(Assembly _assembly)
        {
	        if (_assembly == null)
	        {
		        throw new ArgumentNullException(nameof(_assembly));
	        }
	        var worldPacketsTypes = _assembly.GetTypes().Where(x => typeof(WorldPacket).IsAssignableFrom(x)).Where(x => !x.IsAbstract).ToArray();
            foreach (var worldPacketType in worldPacketsTypes)
            {
                RegisterPacket<WorldPacket>(worldPacketType);
            }

            var clientPacketsTypes = _assembly.GetTypes().Where(x => typeof(ClientPacket).IsAssignableFrom(x)).Where(x => !x.IsAbstract).ToArray();
            foreach (var clientPacketType in clientPacketsTypes)
            {
                RegisterPacket<ClientPacket>(clientPacketType);
            }

            Logger.InfoFormat(
            "[PacketsFactory] Loaded {0} world packet(s) types\r\n\t{1}",
            m_worldPackets.Count,
            String.Join(
                "\r\n\t",
                m_worldPackets.OrderBy(x => (uint)x.Key).Select(kvp => String.Format("0x{2:X8} {0} {1}", kvp.Key, kvp.Value.Name, (uint)kvp.Key))));
            Logger.InfoFormat(
            "[PacketsFactory] Loaded {0} client packet(s) types\r\n\t{1}",
            m_clientPackets.Count,
            String.Join(
                "\r\n\t",
                m_clientPackets.OrderBy(x => (uint)x.Key).Select(kvp => String.Format("0x{2:X8} {0} {1}", kvp.Key, kvp.Value.Name, (uint)kvp.Key))));

        }

        public static PacketsFactory Instance
        {
            get
            {
                return m_instance.Value;
            }
        }

        public bool TryToReadHeader(byte[] _data, out MessageHeader _header)
        {
            _header = null;
            if (_data.Length < MessageHeader.Length)
            {
                return false;
            }

            var header = new MessageHeader();
            header.Parse(_data.Take(MessageHeader.Length).ToArray());
            if (header.MessageLength > _data.Length - 2)
            {
                // not enough data
                return false;
            }

            _header = header;
            return true;
        }

        public BasicPacket Parse(byte[] _data)
        {
            MessageHeader header;
            if (!TryToReadHeader(_data, out header))
            {
                throw new FormatException(string.Format("Invalid packet data\r\n{0}", StringUtils.HexDump(_data)));
            }

            BasicPacket result;
            switch (header.PacketType)
            {
                case SubType.World_Compressed:
                case SubType.World:
                    {
                        WorldPacket worldPacket;
                        if (header.PacketType == SubType.World_Compressed)
                        {
                            _data = UncompressPacket(_data);

                        }

                        if (header.UNC == 0xDD)
                        {
                            worldPacket = WorldPacket.Create(_data);
                            Type worldPacketType;
                            if (m_worldPackets.TryGetValue(worldPacket.OpCode, out worldPacketType))
                            {
                                worldPacket = (WorldPacket)Activator.CreateInstance(worldPacketType);
                                worldPacket.Parse(_data);
                            }
                            result = worldPacket;
                        }
                        else
                        {
                            var clientPacket = ClientPacket.Create(_data);
                            Type clientPacketType;
                            if (m_clientPackets.TryGetValue(clientPacket.OpCode, out clientPacketType))
                            {
                                clientPacket = (ClientPacket)Activator.CreateInstance(clientPacketType);
                                clientPacket.Parse(_data);
                            }
                            result = clientPacket;
                        }
                    }
                    break;
                default:
                    {
                        result = new BasicPacket();
                        result.Parse(_data);
                    } break;

            }

            return result;
        }

        private byte[] UncompressPacket(byte[] _inputData)
        {
	        if (_inputData == null)
	        {
		        throw new ArgumentNullException(nameof(_inputData));
	        }
	        var compressedData = _inputData.Skip(MessageHeader.Length).ToArray();
            var uncompressedData = Utils.UncompressDeflate(compressedData);
            var packet = _inputData.Take(MessageHeader.Length).Concat(uncompressedData).ToArray();
            return packet;
        }
    }
}
