namespace AAShared.Packets.Enums
{
    public enum SubType : byte
    {
        Unknown = 0,
        World = 0x01,
        Proxy = 0x02,
        World_Compressed = 0x03,
        World_multi_compressed = 0x04
    }
}