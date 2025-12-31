using PurrNet.Packing;

namespace PurrNet.Insthync.ResquestResponse
{
    public struct RequestMessage : IPackedAuto
    {
        public ushort requestType;
        public uint requestId;
        public byte[] data;
    }
}