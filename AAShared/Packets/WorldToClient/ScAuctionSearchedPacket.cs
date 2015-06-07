using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

using AAShared.BinaryClasses;
using AAShared.Packets.Enums;

namespace AAShared.Packets.WorldToClient
{
    public class ScAuctionSearchedPacket : WorldPacket
    {
        public IEnumerable<AuctionRecord> Records { get; set; }

        public DateTime RefreshTime { get; set; }

        private static int counter;

        public ScAuctionSearchedPacket():base(WorldOpCode.SCAuctionSearchedPacket)
        {
            
        }

        /// <summary>
        ///   Метод, который вызывается, когда идет запрос на парсинг пакета с использованием указанного диссектора
        /// </summary>
        /// <param name="_dissector"></param>
        protected override void InternalParse(PacketDissector _dissector)
        {
            using (var file = File.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "packets") + counter + ".bin"))
            {
                Interlocked.Increment(ref counter);
                file.Write(_dissector.RawData, 0, _dissector.RawData.Count());
            }

            base.InternalParse(_dissector);
            _dissector.ReadArray<byte>(4);

            var records = new List<AuctionRecord>();
            var itemsCount = _dissector.ReadValue<ushort>();
            _dissector.ReadArray<byte>(2);

            for (int i = 0; i < itemsCount; i++)
            {
                var record = new AuctionRecord();
                record.Parse(_dissector);
                records.Add(record);
            }
            Records = records;

            var unixRefreshTimestamp = _dissector.ReadValue<uint>();
            RefreshTime = Utils.UnixTimeStampToDateTime(unixRefreshTimestamp);
            _dissector.ReadArray<byte>(4);

        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return String.Format("{0}\r\n\t{1}", this.FormatHeader(false), String.Join("\r\n\t", Records ?? new AuctionRecord[0]));
        }
    }
}