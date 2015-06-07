using System;

using AAShared.Packets;
using AAShared.Packets.ClientToWorld;
using AAShared.Packets.Enums;
using AAShared.Packets.WorldToClient;

using XMLib;
using XMLib.Log;

namespace AAShared
{
    public class PacketsProcessor
    {
	    private readonly PacketsBuffer m_source;

	    private uint m_playerId;

        private uint m_targetId;

        public event EventHandler<BuffRemovedEventArgs> BuffRemoved = delegate { };
        public event EventHandler<BuffCreatedEventArgs> BuffCreated = delegate { };

        public event EventHandler<EventArgs<uint>> PlayerIdChanged = delegate { };
        public event EventHandler<EventArgs<uint>> TargetChanged = delegate { };

        public event EventHandler<UnitDamagedEventArgs> UnitDamaged = delegate { };

        public uint PlayerId
        {
            get
            {
                return m_playerId;
            }
        }

        public uint TargetId
        {
            get
            {
                return m_targetId;
            }
        }

        public PacketsProcessor(PacketsBuffer _source)
        {
	        m_source = _source;
	        _source.PacketReceived += PacketsBufferOnPacketReceived;
            _source.PacketSent += BufferOnPacketSent;
        }


        private void BufferOnPacketSent(object _sender, EventArgs<BasicPacket> _eventArgs)
        {
            var packet = _eventArgs.Value;
            if (packet.Header.PacketType == SubType.World)
            {
                if (packet is ClientPacket)
                {
                    var clientPacket = packet as ClientPacket;
                    switch (clientPacket.OpCode)
                    {
                        case ClientOpCode.ChangeTargetPacket:
                            if (packet is ChangeTargetPacket)
                            {
                                ProcessChangeTargetPacket(packet as ChangeTargetPacket);
                            }
                            else
                            {
                                Logger.InfoFormat("Packet sent\r\n{0}", clientPacket);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void PacketsBufferOnPacketReceived(object _sender, EventArgs<BasicPacket> _eventArgs)
        {
            var packet = _eventArgs.Value;
            //Logger.InfoFormat("Packet received\r\n{0}", packet);

            if (packet is WorldPacket)
            {
                var worldPacket = packet as WorldPacket;
                switch (worldPacket.OpCode)
                {
                    case WorldOpCode.SCAuctionBidPacket:
                    case WorldOpCode.SCAuctionCanceledPacket:
                    case WorldOpCode.SCAuctionLowestPricePacket:
                    case WorldOpCode.SCAuctionMessagePacket:
                    case WorldOpCode.SCAuctionPostedPacket:
                    case WorldOpCode.SCAuctionSearchedPacket:
                        Logger.InfoFormat("Auction packet received\r\n{0}", packet);
                        break;

                    case WorldOpCode.TargetChangedPacket:
                    case WorldOpCode.SCBuffCreatedPacket:
                    case WorldOpCode.SCBuffRemovedPacket:
                    case WorldOpCode.SCBuffLearnedPacket:
                    case WorldOpCode.SCBuffStatePacket:
                    case WorldOpCode.SCBuffUpdatedPacket:
                        if (packet is CsTargetChangedPacket)
                        {
                            ProcessTargetChangedPacket(packet as CsTargetChangedPacket);
                        }
                        else if (packet is ScBuffCreatedPacket)
                        {
                            ProcessBuffCreatedPacket(packet as ScBuffCreatedPacket);
                        }
                        else if (packet is ScBuffRemovedPacket)
                        {
                            ProcessBuffRemovedPacket(packet as ScBuffRemovedPacket);
                        }
                        else if (packet is ScUnitDamagedPacket)
                        {
                            ProcessUnitDamagedPacket(packet as ScUnitDamagedPacket);
                        }
                        else
                        {
                            Logger.InfoFormat("Interesting Packet received\r\n{0}", packet);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void ProcessChangeTargetPacket(ChangeTargetPacket _packet)
        {
            if (m_playerId == 0 && _packet.TargetId != 0)
            {
                m_playerId = _packet.TargetId;
                Logger.InfoFormat("[PacketsProcessor.ProcessChangeTargetPacket] Retrieved playerId {0:X6}", m_playerId);
                PlayerIdChanged(this, new EventArgs<uint>(m_targetId));
            }
        }

        private void ProcessBuffCreatedPacket(ScBuffCreatedPacket _packet)
        {
	        if (_packet == null)
	        {
		        throw new ArgumentNullException(nameof(_packet));
	        }
	        if (_packet.TargetId == 0)
            {
                Logger.WarnFormat("[ProcessBuffCreatedPacket] Empty char ?\r\n{0}", _packet);
            }
            else
            {
                if (_packet.TargetId == m_playerId)
                {
                    Logger.InfoFormat("BuffCreated packet\r\n{0}", _packet);
                }
                var buff = new CharacterBuff(_packet.TargetId, _packet.BuffId, _packet.BuffIndex);
                if (buff.Info != null)
                {
                    buff.Duration = buff.Info.Duration;
                }
                BuffCreated(this, new BuffCreatedEventArgs(_packet.TargetId, buff));
            }
        }

	    private void ProcessBuffRemovedPacket(ScBuffRemovedPacket _packet)
        {
	        if (_packet == null)
	        {
		        throw new ArgumentNullException(nameof(_packet));
	        }
	        if (_packet.CharId == 0)
            {
                Logger.WarnFormat("[ProcessBuffRemovedPacket] Empty char ?\r\n{0}", _packet);
            }
            else
            {
                if (_packet.CharId == m_playerId)
                {
                    Logger.InfoFormat("BuffRemoved packet\r\n{0}", _packet);
                }
                BuffRemoved(this, new BuffRemovedEventArgs(_packet.CharId, _packet.BuffIndex));
            }
        }

	    private void ProcessTargetChangedPacket(CsTargetChangedPacket _packet)
	    {
		    if (_packet == null)
		    {
			    throw new ArgumentNullException(nameof(_packet));
		    }

		    if (_packet.CharId == m_playerId)
            {
                if (_packet.TargetId == 0)
                {
                    Logger.InfoFormat("[PacketsProcessor.ProcessTargetChangedPacket] Player cleared target");
                }
                else
                {
                    Logger.InfoFormat("[PacketsProcessor.ProcessTargetChangedPacket] Player is targeting Id {0:X6}", _packet.TargetId);
                }
                m_targetId = _packet.TargetId;
                TargetChanged(this, new EventArgs<uint>(m_targetId));
            }
	    }

	    private void ProcessUnitDamagedPacket(ScUnitDamagedPacket _packet)
        {
		    if (_packet == null)
		    {
			    throw new ArgumentNullException(nameof(_packet));
		    }
		    //Logger.InfoFormat("[PacketsProcessor.ProcessUnitDamagedPacket] Damage event from source {0:X6}\r\n\t{1}",_packet.SourceId,_packet);

            var args = new UnitDamagedEventArgs(_packet.SourceId, _packet.TargetId, _packet.SkillId) { Damage = _packet.Damage, Absorb = _packet.Absorb };
            UnitDamaged(this, args);
        }
    }

    public class BuffRemovedEventArgs : EventArgs
    {
        public uint CharId { get; private set; }

        public uint BuffIndex { get; private set; }

        public BuffRemovedEventArgs(uint _charId, uint _buffIndex)
        {
            CharId = _charId;
            BuffIndex = _buffIndex;
        }
    }

    public class BuffCreatedEventArgs : EventArgs
    {
        public CharacterBuff NewCharacterBuff { get; private set; }
        public uint TargetId { get; private set; }

        public BuffCreatedEventArgs(uint _targetId, CharacterBuff _newCharacterBuff)
        {
            TargetId = _targetId;
            NewCharacterBuff = _newCharacterBuff;
        }
    }

    public class UnitDamagedEventArgs : EventArgs
    {
        public uint SourceId { get; private set; }
        public uint TargetId { get; private set; }

        public uint SkillId { get; private set; }

        public uint Damage { get; set; }

        public uint Absorb { get; set; }

        public UnitDamagedEventArgs(uint _sourceId, uint _targetId, uint _skillid)
        {
            SourceId = _sourceId;
            TargetId = _targetId;
            SkillId = _skillid;
        }
    }
}
