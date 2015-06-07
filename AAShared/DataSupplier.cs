using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using XMLib;
using XMLib.Log;
using XMLib.Statistics;

using SharpPcap;
using SharpPcap.LibPcap;
using SharpPcap.WinPcap;

namespace AAShared
{
    public class DataSupplier
    {
        private IEnumerable<WinPcapDevice> m_devices;

        public event EventHandler<EventArgs<byte[]>> RawPacketReceived = delegate { };
        public event EventHandler<EventArgs<byte[]>> RawPacketSent = delegate { };

        private IEnumerable<IPAddress> m_addresses;

        private Thread m_thread;
        private ManualResetEvent m_threadCancelEvent;

		private BlockingCollection<RawCapture> m_packets;

        private DateTime m_lastStatsTimestamp;

        public DataSupplier()
        {


            // Retrieve the device list
            var allDevices = CaptureDeviceList.Instance.OfType<WinPcapDevice>();
            foreach (var dev in allDevices)
            {
                Logger.DebugFormat("{0}", dev.Description);

                foreach (PcapAddress addr in dev.Addresses.Where(x => x.Addr != null && x.Addr.ipAddress != null))
                {
                    if (addr.Addr != null && addr.Addr.ipAddress != null && addr.Addr.ipAddress.AddressFamily == AddressFamily.InterNetwork)
                    {
                        Logger.DebugFormat("{0}", addr.Addr.ipAddress);
                    }
                }
            }

            // If no devices were found print an error
            if (!allDevices.Any())
            {
                Logger.ErrorFormat("No devices were found on this machine");
                return;
            }

            m_devices = allDevices.Where(x => !x.Loopback && x.Name.IndexOf("vm", StringComparison.InvariantCultureIgnoreCase) < 0);
            m_addresses =
                m_devices.SelectMany(device =>
                    device.Addresses
                        .Where(x => x.Addr != null && x.Addr.ipAddress != null)
                        .Where(x => x.Addr.ipAddress.AddressFamily == AddressFamily.InterNetwork).Select(x => x.Addr.ipAddress)).Distinct().ToArray();
        }

        private void DeviceOnOnPacketArrival(object _sender, CaptureEventArgs _captureEventArgs)
        {
            m_packets.Add(_captureEventArgs.Packet);
        }

        public void Start()
        {
            m_packets = new BlockingCollection<RawCapture>();
            m_thread = new Thread(WorkerMethod) { Name = "RawPckts", IsBackground = true};
			m_threadCancelEvent=  new ManualResetEvent(false);
            m_thread.Start();
            foreach (var device in m_devices.Where(x => x.Addresses != null))
            {
                var ipAddress = device.Addresses
                    .Where(x => x.Addr != null && x.Addr.ipAddress != null)
                    .Where(x => x.Addr.ipAddress.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault();
                if (ipAddress == null)
                {
                    continue;
                }
                // Register our handler function to the 'packet arrival' event
                device.OnPacketArrival += DeviceOnOnPacketArrival;
                // Open the device for capturing
                int readTimeoutMilliseconds = 100;
                var winPcap = device as WinPcapDevice;
                winPcap.Open(SharpPcap.WinPcap.OpenFlags.DataTransferUdp | SharpPcap.WinPcap.OpenFlags.NoCaptureLocal, readTimeoutMilliseconds);
                winPcap.KernelBufferSize = 32 * 1024 * 1024;
                Console.WriteLine();
                Console.WriteLine("-- Listening on {0} {1}, hit 'Enter' to stop...",
                    device.Name, device.Description);
                device.Filter = string.Format("tcp and port 1239 ", ipAddress.Addr);

                // Start the capturing process
                device.StartCapture();
            }
        }

        private void WorkerMethod()
        {
            do
            {
                RawCapture rawPacket;
                while (m_packets.TryTake(out rawPacket))
                {
                    var now = DateTime.Now;
                    if (now - m_lastStatsTimestamp > TimeSpan.FromSeconds(60))
                    {
                        Logger.InfoFormat("[DataSupplier] Stats(buf size - {1}):\r\n\t{0}",
                      String.Join("\r\n\t",m_devices.Where(x => x.Opened).Select(x => x.Statistics)), 
                      m_packets.Count
                      );
                        m_lastStatsTimestamp = now;
                    }


                    var packet = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
                    var tcpPacket = PacketDotNet.TcpPacket.GetEncapsulated(packet);
                    if (tcpPacket != null)
                    {
                        var ipPacket = (PacketDotNet.IpPacket)tcpPacket.ParentPacket;
                        System.Net.IPAddress srcIp = ipPacket.SourceAddress;
                        System.Net.IPAddress dstIp = ipPacket.DestinationAddress;

                        var data = tcpPacket.PayloadData;
                        if (data.Any())
                        {
                            if (m_addresses.Contains(srcIp))
                            {
                                RawPacketSent(this, new EventArgs<byte[]>(data));
                            }
                            else
                            {
                                RawPacketReceived(this, new EventArgs<byte[]>(data));
                            }
                        }
                    }

                }
            } while (!m_threadCancelEvent.WaitOne(0));
        }

        public void Stop()
        {
            foreach (var device in m_devices)
            {
                // Stop the capturing process
                device.StopCapture();

                // Close the pcap device
                device.Close();
            }
        }
    }
}
