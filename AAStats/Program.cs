using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

using AAShared;
using AAShared.Database;
using AAShared.Packets;
using AAShared.Packets.Enums;

using XMLib;
using XMLib.Log;
using XMLib.Reflection;

using PacketDotNet;

using SharpPcap;
using SharpPcap.AirPcap;
using SharpPcap.LibPcap;
using SharpPcap.WinPcap;

using XMLib.Extensions;

namespace AAStats
{
    static class Program
    {
        private static IEnumerable<WorldOpCode> m_ignoreList = new[]
        {
            WorldOpCode.SCSkillControllerStatePacket,

        };

        [STAThread]
        public static void Main(string[] args)
        {
            Logger.InitializeLocalLogger();
            Logger.InitializeColoredConsoleLogger();
            Logger.DumpApplicationInfo();
            Logger.DumpLibrariesInfo();

            Logger.DebugFormat("{0}",Properties.Settings.Default.DumpProperties());

            PacketsFactory.RegisterPacketsFromExecutingAssembly();

            AADb.Instance.LoadDefaults();

            var dataSupplier = new DataSupplier();

            var buffer = new RawPacketsRetranslator(dataSupplier);
            var summator = new PacketsBuffer(buffer);


            var processor = new PacketsProcessor(summator);

            var game = new Game(processor);

            Logger.InfoFormat("Starting sniffer...");
            dataSupplier.Start();

            var handle = Utils.GetConsoleWindow();
            Utils.ShowWindow(handle, Utils.SW_HIDE);
        }

    }
}
