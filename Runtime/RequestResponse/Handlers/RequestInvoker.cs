using PurrNet.Packing;

namespace PurrNet.Insthync.ResquestResponse
{
    public interface IRequestInvoker
    {
        void InvokeRequest(RequestHandlerData requestHandler);
    }

    public class RequestInvoker<TRequest, TResponse> : IRequestInvoker
        where TRequest : IPacked, new()
        where TResponse : IPacked, new()
    {
        private RequestResponseHandler _handler;
        private RequestDelegate<TRequest, TResponse> _requestHandler;

        public RequestInvoker(RequestResponseHandler handler, RequestDelegate<TRequest, TResponse> requestHandler)
        {
            _handler = handler;
            _requestHandler = requestHandler;
        }

        public void InvokeRequest(RequestHandlerData requestHandlerData)
        {
            TRequest request = new TRequest();
            if (requestHandlerData.Reader != null)
                request.Read(requestHandlerData.Reader);
            if (_requestHandler != null)
                _requestHandler.Invoke(requestHandlerData, request, (responseCode, response, extraResponseSerializer) => RequestProceeded(requestHandlerData.AsServer, requestHandlerData.PlayerId, requestHandlerData.RequestId, responseCode, response, extraResponseSerializer));
        }

        /// <summary>
        /// Send response to the requester
        /// </summary>
        /// <param name="asServer"></param>
        /// <param name="playerId"></param>
        /// <param name="requestId"></param>
        /// <param name="responseCode"></param>
        /// <param name="response"></param>
        /// <param name="extraResponseSerializer"></param>
        private void RequestProceeded(bool asServer, PlayerID playerId, uint requestId, ResponseCode responseCode, TResponse response, SerializerDelegate extraResponseSerializer)
        {
            // Write response
            _handler.Writer.ResetPosition();
            response.Write(_handler.Writer);
            if (extraResponseSerializer != null)
                extraResponseSerializer.Invoke(_handler.Writer);
            ResponseMessage responseMessage = new ResponseMessage()
            {
                requestId = requestId,
                responseCode = responseCode,
                data = _handler.Writer.ToByteData().span.ToArray(),
            };
            if (asServer)
            {
                // Request received at server, so response to client
                _handler.Manager.NetworkManager.Send(playerId, responseMessage);
            }
            else
            {
                // Request received at client, so response to server
                _handler.Manager.NetworkManager.SendToServer(responseMessage);
            }
        }
    }
}