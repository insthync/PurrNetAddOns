using PurrNet.Packing;

namespace PurrNet.Insthync.ResquestResponse
{
    public class RequestHandlerData
    {
        public ushort RequestType { get; private set; }
        public uint RequestId { get; private set; }
        public RequestResponseHandler ReqResHandler { get; private set; }
        public bool AsServer { get; private set; }
        public PlayerID PlayerId { get; private set; }
        public BitPacker Reader { get; private set; }

        public RequestHandlerData(ushort requestType, uint requestId, RequestResponseHandler reqResHandler, bool asServer, PlayerID playerId, BitPacker reader)
        {
            RequestType = requestType;
            RequestId = requestId;
            ReqResHandler = reqResHandler;
            AsServer = asServer;
            PlayerId = playerId;
            Reader = reader;
        }
    }
}
