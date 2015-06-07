using System;
using System.IO;
using System.Linq;
using System.Text;

using XMLib;

namespace AAShared.BinaryClasses
{
    public abstract class BinaryEncoderDecoder
    {
        protected Encoding m_encoding;

        public static Encoding DefaultEncoding
        {
            get
            {
                return Encoding.UTF8;
            }
        }
    }

    /// <summary>
    ///   Класс-обертка для упрощенной записи некоторых видов данных
    /// </summary>
    public class PacketCompiler : BinaryEncoderDecoder
    {
        protected MemoryStream m_buffer;

        public PacketCompiler(Encoding _encoding)
        {
	        if (_encoding == null)
	        {
		        throw new ArgumentNullException(nameof(_encoding));
	        }
	        m_encoding = _encoding;
            m_buffer = new MemoryStream();
        }

        public PacketCompiler()
            : this(DefaultEncoding)
        {
        }

        public void Write<T>(T _value)
        {
            if (typeof(T).IsEnum)
            {
                var underlayingTypeCode = Type.GetTypeCode(typeof(T));
                switch (underlayingTypeCode)
                {
                    case TypeCode.Byte:
                        {
                            var value = Convert.ToByte(_value);
                            Write(value);
                        }
                        break;
                    case TypeCode.UInt16:
                        {
                            var value = Convert.ToUInt16(_value);
                            Write(value);
                        }
                        break;
                    case TypeCode.Int16:
                        {
                            var value = Convert.ToInt16(_value);
                            Write(value);
                        }
                        break;
                    case TypeCode.Int32:
                        {
                            var value = Convert.ToInt32(_value);
                            Write(value);
                        }
                        break;
                    case TypeCode.UInt32:
                        {
                            var value = Convert.ToUInt32(_value);
                            Write(value);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(String.Format("Unsupported enum typecode - {0}, enum {1}", underlayingTypeCode, _value));
                }

            }
            else
            {
                var bytes = ToByteArray(_value);
                WriteBytes(bytes);
            }
        }

        public byte[] ToByteArray<T>(T _value)
        {
	        if (_value == null)
	        {
		        throw new ArgumentNullException(nameof(_value));
	        }
	        object val = _value;
            byte[] bytes;
            if (typeof(T) == typeof(byte[]))
            {
                return (byte[])val;
            }

            var typecode = Type.GetTypeCode(typeof(T));
            switch (typecode)
            {
                case TypeCode.Boolean:
                    bytes = BitConverter.GetBytes((bool)val);
                    break;
                case TypeCode.Char:
                    bytes = BitConverter.GetBytes((char)val);
                    break;
                case TypeCode.String:
                    bytes = m_encoding.GetBytes((string)val).Concat(new byte[] { 0 }).ToArray();
                    break;
                case TypeCode.Byte:
                    bytes = new[] { (byte)val };
                    break;
                case TypeCode.Int16:
                    bytes = BitConverter.GetBytes((short)val);
                    break;
                case TypeCode.UInt16:
                    bytes = BitConverter.GetBytes((ushort)val);
                    break;
                case TypeCode.Int32:
                    bytes = BitConverter.GetBytes((int)val);
                    break;
                case TypeCode.UInt32:
                    bytes = BitConverter.GetBytes((uint)val);
                    break;
                case TypeCode.DateTime:
                    {
                        var dateTime = (DateTime)val;
                        var secondsSinceUnix = (uint)XMLib.Extensions.DateTimeUtils.ToMillisecondsSinceUnixEpoch(dateTime);
                        bytes = BitConverter.GetBytes(secondsSinceUnix);
                    }
                    break;
                default:
                    throw new NotSupportedException(String.Format("Type(typecode {1}) {0} is not supported by ToByteArray<T>", typeof(T), typecode));
            }
            return bytes;
        }

        public void WriteBytes(byte[] _byteArr)
        {
	        if (_byteArr == null)
	        {
		        throw new ArgumentNullException(nameof(_byteArr));
	        }
	        m_buffer.Write(_byteArr, 0, _byteArr.Length);
        }

	    public byte[] Compile()
        {
            return m_buffer.ToArray();
        }
    }
}
