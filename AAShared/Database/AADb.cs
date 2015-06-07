using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

using CsvHelper;

using XMLib.Log;

namespace AAShared.Database
{
    public class AADb
    {
        private static Lazy<AADb> m_instance = new Lazy<AADb>();

        private ConcurrentDictionary<uint, BuffInfo> m_buffs;

        private ConcurrentDictionary<uint, Image> m_icons;

        public Size DefaultImgSize = new Size(48,48);

        public static AADb Instance
        {
            get
            {
                return m_instance.Value;
            }
        }

        public bool TryToGetBuff(uint _id, out BuffInfo _buff)
        {
            return m_buffs.TryGetValue(_id, out _buff);
        }

        public bool TryToGetBuffIcon(uint _id, out Image _buffIcon)
        {
            return m_icons.TryGetValue(_id, out _buffIcon);
        }

        public AADb()
        {
            m_buffs = new ConcurrentDictionary<uint, BuffInfo>();
            m_icons = new ConcurrentDictionary<uint, Image>();
        }

        public void LoadDefaults()
        {
            var fileName = "buffs.csv";
            var dbDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database");
            var dbPath = Path.Combine(dbDirectory, fileName);
            var iconsPath = Path.Combine(dbDirectory, "png");
            this.LoadFromCsv(dbPath);
            this.LoadImages(iconsPath);
        }

        public void LoadFromCsv(string _filePath)
        {
            if (!File.Exists(_filePath))
            {
                throw new FileNotFoundException("File not found !", _filePath);
            }
            m_buffs = new ConcurrentDictionary<uint, BuffInfo>();
            Logger.DebugFormat("[AADb.LoadFromCsv] Loading buffs from '{0}'...", _filePath);
            using (var reader = new CsvReader(new StreamReader(File.OpenRead(_filePath))))
            {
                while (reader.Read())
                {
                    var buff = new BuffInfo();
                    buff.Id = (uint)reader.GetField<int>("id");
                    buff.Name = reader.GetField<string>("name");
                    buff.IconName = reader.GetField<string>("filename");
                    buff.Duration = TimeSpan.FromMilliseconds(reader.GetField<int>("duration"));
                    buff.Kind = (BuffKind)reader.GetField<int>("kind");
                    m_buffs.TryAdd(buff.Id, buff);
                };
            }
            Logger.DebugFormat("[AADb.LoadFromCsv] Loaded {0} buff(s)", m_buffs.Count);
        }

        public void LoadImages(string _iconsPath)
        {
            if (!Directory.Exists(_iconsPath))
            {
                throw new DirectoryNotFoundException(message: string.Format("Dir not found '{0}'", _iconsPath));
            }
            m_icons = new ConcurrentDictionary<uint, Image>();

            Logger.DebugFormat("[AADb.LoadImages] Loading icons from '{0}'", _iconsPath);

            var paths = new List<string>();
            foreach (var buffInfo in m_buffs)
            {
                var buffId = buffInfo.Key;
                var buffIcon = buffInfo.Value.IconName;

                var iconPathDds = Path.Combine(_iconsPath, buffIcon);
                var iconPath = Path.ChangeExtension(iconPathDds, "png");
                if (File.Exists(iconPath))
                {
                    var loadedIcon = Image.FromFile(iconPath);
                    var resizedImage = ResizeImage(loadedIcon, DefaultImgSize);
                    m_icons.TryAdd(buffId, resizedImage);
                    buffInfo.Value.BuffIcon = resizedImage;
                    paths.Add(iconPath);
                }
            }

            Logger.DebugFormat("[AADb.LoadImages] Loaded {0} icon(s)", m_buffs.Count);
        }

        private static Image ResizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }


    }

    public class BuffInfo
    {
        public uint Id;

        public string Name;

        public string IconName;

        public TimeSpan Duration;

        public Image BuffIcon;

        public BuffKind Kind;
    }

    public enum BuffKind
    {
        Unknown,
        Buff,
        Debuff
    }
}
