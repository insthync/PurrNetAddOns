namespace PurrNet.Insthync.ResquestResponse
{
    public delegate void RequestProceededDelegate<TResponse>(PlayerID playerId, uint requestId, ResponseCode responseCode, TResponse response, SerializerDelegate extraResponseSerializer);
}
