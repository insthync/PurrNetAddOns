namespace PurrNet.Insthync.ResquestResponse
{
    public delegate void RequestProceedResultDelegate<TResponse>(ResponseCode responseCode, TResponse response, SerializerDelegate responseExtraSerializer = null);
}
