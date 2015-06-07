using System;

using AAShared.BinaryClasses;

using XMLib.Reflection;
using XMLib;
using XMLib.Extensions;

namespace AAShared.Packets.Enums
{
    public class AuctionRecord : BinaryPacket
    {
        public uint Id { get; set; }

        public uint ItemId { get; set; }
        public uint BuyoutPrice { get; set; }
        public uint BidPrice { get; set; }
        public string OwnerName { get; set; }
        public DateTime Timestamp { get; set; }

        /// <summary>
        ///   Метод, который вызывается, когда идет запрос на парсинг пакета с использованием указанного диссектора
        /// </summary>
        /// <param name="_dissector"></param>
        protected override void InternalParse(PacketDissector _dissector)
        {
            Id = _dissector.ReadValue<ushort>();
            /*
             short id;
             short something;
             FSkip(4);
             byte unknownCat;
             long itemId;
             long unknownNumbers;
             byte someIndex;

             local ubyte indexVar;
             while(indexVar != 0xFF)
             {
                 indexVar = ReadUByte( FTell() );
                 FSkip(1);
             //FSkip(0x2c); // 95
             }
             short unknownId;
             short SomeType;
             short nameLength;
             char charName[ nameLength ];
             //48 in bad // 99 in good
             long bidPrice;
             long buyoutPrice;
             time_t bidTime;
             FSkip(19);
             */
            _dissector.Skip(2);
            _dissector.Skip(4);
            _dissector.Skip(1);
            ItemId = _dissector.ReadValue<uint>();
            _dissector.Skip(4);
            _dissector.Skip(1);

            byte someIndex = 0;
            while (someIndex != 0xFF)
            {
                someIndex = _dissector.ReadValue<byte>();
            }
            _dissector.Skip(2);
            _dissector.Skip(2);

            var nameLength = _dissector.ReadValue<short>();
            OwnerName = _dissector.ReadString(nameLength);
            
            BidPrice = _dissector.ReadValue<uint>();
            BuyoutPrice = _dissector.ReadValue<uint>();
            
            var unixTimestamp = _dissector.ReadValue<uint>();
            Timestamp = Utils.UnixTimeStampToDateTime(unixTimestamp);
            
            _dissector.ReadArray<byte>(19); 
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}", this.DumpProperties(true));
        }
    }
}