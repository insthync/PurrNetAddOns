using PurrNet.Packing;

namespace PurrNet.Insthync.ResquestResponse
{
    public struct ResponseMessage : IPackedAuto
    {
        public uint requestId;
        public ResponseCode responseCode;
        public byte[] data;
    }
}