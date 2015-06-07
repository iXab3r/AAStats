#region Usings

using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text;

using ICSharpCode.SharpZipLib.Zip.Compression;

using XMLib;

#endregion

namespace AAShared
{
    public class Utils
    {
        public const int SW_HIDE = 0;

        public const int SW_SHOW = 5;

        public const int GWL_STYLE = (-16);

        public const int GWL_EXSTYLE = (-20);

        public const UInt32 WS_POPUP = 0x80000000;

        public const UInt32 WS_CHILD = 0x40000000;

        public const UInt32 WS_EX_LAYERED = 0x80000;

        public const UInt32 WS_EX_TRANSPARENT = 0x20;

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static Bitmap Copy(Bitmap srcBitmap, Rectangle section)
        {
            // Create the new bitmap and associated graphics object
            var bmp = new Bitmap(section.Width, section.Height);
            var g = Graphics.FromImage(bmp);

            // Draw the specified section of the source bitmap to the new one
            g.DrawImage(srcBitmap, 0, 0, section, GraphicsUnit.Pixel);

            // Clean up
            g.Dispose();

            // Return the bitmap
            return bmp;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FreeConsole();

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            var Buff = new StringBuilder(nChars);
            var handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return String.Empty;
        }

        public static void SetWindowExTransparent(IntPtr hwnd)
        {
            var extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
        }

        public static string TimeSpanToHumanReadableString(TimeSpan _ts)
        {
            var seconds = (int)_ts.TotalSeconds;
            if (_ts == default(TimeSpan))
            {
                return string.Empty;
            }
            else if (seconds <= 5)
            {
                return String.Format("{0:0.0}", _ts.TotalSeconds);
            }
            else if (seconds <= 90)
            {
                return String.Format("{0}s", (int)_ts.TotalSeconds);
            }
            else if (seconds <= 90 * 60)
            {
                return String.Format("{0}m", (int)_ts.TotalMinutes);
            }
            else
            {
                return String.Format("{0}h", (int)_ts.TotalHours);
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool TerminateProcess(IntPtr _hProcess, uint _uExitCode);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, UInt32 dwNewLong);

        private static byte[] UncompressDeflate_WRONG(byte[] _bytes)
        {
	        if (_bytes == null)
	        {
		        throw new ArgumentNullException(nameof(_bytes));
	        }
	        using (var ds = new DeflateStream(new MemoryStream(_bytes), CompressionMode.Decompress))
            {
                using (var ms = new MemoryStream())
                {
                    ds.CopyTo(ms);
                    ds.Close();
                    return ms.ToArray();
                }
            }

            var inflate = new Inflater();
        }


        /*
         
         using (MemoryStream memoryStream1 = new MemoryStream(byte_0))
      {
        using (MemoryStream memoryStream2 = new MemoryStream())
        {
          byte[] buffer1 = new byte[32768];
          byte[] buffer2 = new byte[32768];
          while (memoryStream1.Position < (long) byte_0.Length)
          {
            int count1 = memoryStream1.Read(buffer1, 0, buffer1.Length);
            inflater_0.SetInput(buffer1, 0, count1);
            while (!inflater_0.IsNeedingInput)
            {
              int count2 = inflater_0.Inflate(buffer2, 0, buffer2.Length);
              if (count2 != 0)
                memoryStream2.Write(buffer2, 0, count2);
              else
                break;
            }
            if (inflater_0.IsFinished)
              break;
          }
          return memoryStream2.ToArray();
        }
      }
         */

        public static byte[] UncompressDeflate(byte[] _inputData)
        {
            var inflater = new Inflater(true);
            using (var inputStream = new MemoryStream(_inputData))
            using (var outputStream = new MemoryStream())
            {
                var inputBuffer = new byte[short.MaxValue];
                var outputBuffer = new byte[short.MaxValue];
                while (inputStream.Position < (long)_inputData.Length)
                {
                    var bytesRead = inputStream.Read(inputBuffer, 0, inputBuffer.Length);
                    inflater.SetInput(inputBuffer, 0, bytesRead);
                    while (!inflater.IsNeedingInput)
                    {
                        var bytesDecompressed = inflater.Inflate(outputBuffer, 0, outputBuffer.Length);
                        if (bytesDecompressed != 0)
                        {
                            outputStream.Write(outputBuffer, 0, bytesDecompressed);
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (inflater.IsFinished)
                    {
                        break;
                    }
                }
                inflater.Reset();
                return outputStream.ToArray();
            }
        }
    }
}