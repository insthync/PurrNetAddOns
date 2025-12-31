using PurrNet.Packing;

namespace PurrNet.Insthync.ResquestResponse
{
    public class RequestCallback
    {
        public uint RequestId { get; private set; }
        public RequestResponseHandler ReqResHandler { get; private set; }
        public IResponseInvoker ResponseInvoker { get; private set; }
        public ResponseDelegate<object> ResponseHandler { get; private set; }

        public RequestCallback(
            uint requestId,
            RequestResponseHandler reqResHandler,
            IResponseInvoker responseInvoker,
            ResponseDelegate<object> responseHandler)
        {
            RequestId = requestId;
            ReqResHandler = reqResHandler;
            ResponseInvoker = responseInvoker;
            ResponseHandler = responseHandler;
        }

        public void ResponseTimeout()
        {
            ResponseInvoker.InvokeResponse(new ResponseHandlerData(RequestId, ReqResHandler, null, null), ResponseCode.Timeout, ResponseHandler);
        }

        public void Response(PlayerID playerId, BitPacker reader, ResponseCode responseCode)
        {
            ResponseInvoker.InvokeResponse(new ResponseHandlerData(RequestId, ReqResHandler, playerId, reader), responseCode, ResponseHandler);
        }
    }
}
