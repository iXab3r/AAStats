using System;

using AAShared.BinaryClasses;

using XMLib;

namespace AAShared.Packets
{

    /// <summary>
    ///   Абстракатный класс, который представляет собой обертку над любыми бинарными данными, которые можно компилировать/парсить. 
    ///   Все пакеты CTI, заголовки и т.п. наследуют этот класс. 
    /// </summary>
    public abstract class BinaryPacket
    {
        /// <summary>
        ///   Метод, который вызывается, когда идет запрос на парсинг пакета с использованием указанного диссектора
        /// </summary>
        /// <param name="_dissector"></param>
        protected virtual void InternalParse(PacketDissector _dissector)
        {
	        if (_dissector == null)
	        {
		        throw new ArgumentNullException(nameof(_dissector));
	        }
        }

	    /// <summary>
        ///   Метод, который вызывается когда идет запрос на компиляцию пакета с использованеим указанного компилятора
        /// </summary>
        /// <param name="_compiler"></param>
        protected virtual void InternalCompile(PacketCompiler _compiler)
	    {
		    if (_compiler == null)
		    {
			    throw new ArgumentNullException(nameof(_compiler));
		    }
	    }

	    /// <summary>
        ///  Выполняет парсинг байтового массива посредством стандартного диссектора.
        /// </summary>
        /// <param name="_packetData"></param>
        public  void Parse(byte[] _packetData)
        {
		    if (_packetData == null)
		    {
			    throw new ArgumentNullException(nameof(_packetData));
		    }
            Parse(new PacketDissector(_packetData));
        }

        /// <summary>
        ///   Выполняет парсинг данных, используя предоставленный диссектор
        /// </summary>
        /// <param name="_dissector"></param>
        public virtual void Parse(PacketDissector _dissector)
        {
	        if (_dissector == null)
	        {
		        throw new ArgumentNullException(nameof(_dissector));
	        }
            InternalParse(_dissector);
        }

        /// <summary>
        ///   Выполняет компиляцию данных посредством стандартного комплиятора CTI пакетов
        /// </summary>
        /// <returns></returns>
        public virtual byte[] Compile()
        {
            var compiler = new PacketCompiler();
            InternalCompile(compiler);
            var compiledData = compiler.Compile();
            return compiledData;
        }
    }
}