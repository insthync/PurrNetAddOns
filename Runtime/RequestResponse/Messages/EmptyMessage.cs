using PurrNet.Packing;

namespace PurrNet.Insthync.ResquestResponse
{
    public struct EmptyMessage : IPacked
    {
        public static readonly EmptyMessage Value = new EmptyMessage();

        public void Read(BitPacker packer)
        {
            // Do nothing, it's empty
        }

        public void Write(BitPacker packer)
        {
            // Do nothing, it's empty
        }
    }
}
