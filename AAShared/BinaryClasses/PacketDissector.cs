using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using XMLib;

namespace AAShared.BinaryClasses
{
    /// <summary>
    ///   Класс-обертка над байтовым массивом. Предназначен для обработки и парсинга произвольных пакетов данных.
    /// </summary>
    public class PacketDissector : BinaryEncoderDecoder
    {
        protected readonly byte[] m_rawData;

        protected readonly BinaryReader m_reader;
        protected readonly MemoryStream m_stream;


        public byte[] RawData {
            get
            {
                return m_rawData;
            }
        }

        public long Position {
            get
            {
                return m_stream.Position;
            }
        }

        public long Length
        {
            get
            {
                return m_stream.Length;
            }
        }

        public bool EndOfStream
        {
            get
            {
                return m_stream.Position == m_stream.Length;
            }
        }

        public PacketDissector(byte[] _rawData, Encoding _encoding)
        {
	        if (_rawData == null)
	        {
		        throw new ArgumentNullException(nameof(_rawData));
	        }
	        if (_encoding == null)
	        {
		        throw new ArgumentNullException(nameof(_encoding));
	        }
	        m_rawData = _rawData;
            m_encoding = _encoding;
            m_stream = new MemoryStream(_rawData, false);
            m_reader = new BinaryReader(m_stream);
        }

        public PacketDissector(byte[] _rawData)
            : this(_rawData, DefaultEncoding)
        {
        }

        public T ReadValue<T>()
        {
            try
            {
                var type = typeof(T);
                var readType = type.IsEnum ? type.GetEnumUnderlyingType() : type;
                object result = default(T);
                result = ReadRawValue(readType);
                if (!type.IsEnum && !(result is T))
                {
                    throw new ApplicationException(String.Format("Expected type '{0}' as result, received '{1}'",typeof(T),result.GetType()));
                }
                return (T)result;
            }
            catch (Exception e)
            {
                throw new ApplicationException(PrepareExceptionMessage(), e);
            }
        }

        public uint ReadId()
        {
            var shortId = ReadArray<byte>(3).Concat(new Byte[] { 0 }).ToArray();
            return BitConverter.ToUInt32(shortId, 0);
        }

        public void Skip(int _bytesCount)
        {
            m_stream.Seek(_bytesCount, SeekOrigin.Current);
        }

        public T[] ReadArray<T>(int _count)
        {
            if (_count < 0)
            {
                throw new ArgumentOutOfRangeException("_count","Count must be >= 0(_count="+_count+")");
            }
            var result = new List<T>();
            for (int i = 0; i < _count; i++)
            {
                result.Add(ReadValue<T>());
            }
            return result.ToArray();
        }

        private object ReadRawValue(Type _type)
        {
	        if (_type == null)
	        {
		        throw new ArgumentNullException(nameof(_type));
	        }
	        object result;
            var typeCode = Type.GetTypeCode(_type);
            switch (typeCode)
            {
                case TypeCode.Byte:
                {
                    result = m_reader.ReadByte();
                }break;
                
                case TypeCode.Int16:
                    {
                        result = m_reader.ReadInt16();
                    }
                    break;
                case TypeCode.UInt16:
                    {
                        result = m_reader.ReadUInt16();
                    }
                    break;
                case TypeCode.Int32:
                    {
                        result = m_reader.ReadInt32();
                    }
                    break;
                case TypeCode.UInt32:
                    {
                        result = m_reader.ReadUInt32();
                    }
                    break;
                case TypeCode.Boolean:
                    {
                        var boolValue = m_reader.ReadUInt16();
                        if (boolValue > 1)
                        {
                            throw new ArgumentOutOfRangeException(String.Format("Expected values 0 or 1 for Boolean, received {0}", boolValue));
                        }
                        result = boolValue == 1;
                    } break;
                case TypeCode.DateTime:
                    {
                        var secondsSince1970 = m_reader.ReadUInt32();
                        var utcTime = XMLib.Extensions.DateTimeUtils.FromSecondsSinceUnixEpoch(secondsSince1970);
                        result = utcTime.ToLocalTime();
                    }
                    break;
                case TypeCode.String:
                    {
                        var byteString = new List<byte>();
                        byte charByte;
                        while ((charByte = m_reader.ReadByte()) != 0)
                        {
                            byteString.Add(charByte);
                        }
                        result = m_encoding.GetString(byteString.ToArray());
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(String.Format("Unsupported type - {0}, typeCode - {1}", _type, typeCode));
            }
            return result;
        }

        public string ReadString(int _length)
        {
            var byteString = m_reader.ReadBytes(_length);
            var result = m_encoding.GetString(byteString);
            return result;
        }

        /// <summary>
        ///   Возвращает C-строку из массива байт
        /// </summary>
        /// <param name="_rawBytes"></param>
        /// <returns></returns>
        public string DecodeNullTerminatedString(byte[] _rawBytes)
        {
	        if (_rawBytes == null)
	        {
		        throw new ArgumentNullException(nameof(_rawBytes));
	        }
	        return m_encoding.GetString(_rawBytes).TrimEnd((Char)0);
        }

	    private string PrepareExceptionMessage()
        {
            var str = new StringBuilder(String.Format("Exception occured during dissecting packet data({0}b)\r\n",m_rawData.Length));
            str.AppendFormat("Encoding: {0}\r\n", m_encoding);
            str.AppendFormat("Reader info: {0} / {1}\r\n", m_stream.Position, m_stream.Length);
            str.AppendFormat("Data\r\n{0}\r\n", StringUtils.HexDump(m_rawData));
            return str.ToString();
        }

        public override string ToString()
        {
            return String.Format("Dissector({0}) {1}/{2}",m_encoding, m_stream.Position, m_stream.Length);
        }
    }
}