using PurrNet.Packing;
using System;

namespace PurrNet.Insthync.ResquestResponse
{
    public interface IResponseInvoker
    {
        void InvokeResponse(ResponseHandlerData responseHandlerData, ResponseCode responseCode, ResponseDelegate<object> responseHandler);
        bool IsRequestTypeValid(Type type);
    }

    public class ResponseInvoker<TRequest, TResponse> : IResponseInvoker
        where TRequest : IPacked, new()
        where TResponse : IPacked, new()
    {
        private ResponseDelegate<TResponse> _responseDelegate;

        public ResponseInvoker(ResponseDelegate<TResponse> responseDelegate)
        {
            _responseDelegate = responseDelegate;
        }

        public void InvokeResponse(ResponseHandlerData responseHandlerData, ResponseCode responseCode, ResponseDelegate<object> responseHandler)
        {
            TResponse response = new TResponse();
            if (responseCode != ResponseCode.Timeout &&
                responseCode != ResponseCode.Unimplemented)
            {
                if (responseHandlerData.Reader != null)
                    response.Read(responseHandlerData.Reader);
            }
            if (_responseDelegate != null)
                _responseDelegate.Invoke(responseHandlerData, responseCode, response);
            if (responseHandler != null)
                responseHandler.Invoke(responseHandlerData, responseCode, response);
        }

        public bool IsRequestTypeValid(Type type)
        {
            return typeof(TRequest) == type;
        }
    }
}
