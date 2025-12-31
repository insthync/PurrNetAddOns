using PurrNet.Packing;

namespace PurrNet.Insthync.ResquestResponse
{
    public class ResponseHandlerData
    {
        public uint RequestId { get; private set; }
        public RequestResponseHandler ReqResHandler { get; private set; }
        public PlayerID? PlayerId { get; private set; }
        public BitPacker Reader { get; private set; }

        public ResponseHandlerData(uint requestId, RequestResponseHandler reqResHandler, PlayerID? playerId, BitPacker reader)
        {
            RequestId = requestId;
            ReqResHandler = reqResHandler;
            PlayerId = playerId;
            Reader = reader;
        }
    }
}
