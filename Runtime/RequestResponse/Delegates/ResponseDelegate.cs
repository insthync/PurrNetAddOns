namespace PurrNet.Insthync.ResquestResponse
{
    public delegate void ResponseDelegate<TResponse>(ResponseHandlerData requestHandler, ResponseCode responseCode, TResponse response);
}
