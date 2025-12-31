using PurrNet;
using PurrNet.Packing;
using System.Threading.Tasks;
using UnityEngine;

namespace PurrNet.Insthync.ResquestResponse
{
    public class RequestResponseManager : MonoBehaviour
    {
        [SerializeField]
        private NetworkManager _networkManager;
        public NetworkManager NetworkManager => _networkManager;

        [SerializeField]
        public int _clientRequestTimeoutInMilliseconds = 30000;

        [SerializeField]
        public int _serverRequestTimeoutInMilliseconds = 30000;

        private RequestResponseHandler _serverReqResHandler;
        private RequestResponseHandler _clientReqResHandler;

        private void Awake()
        {
            if (_networkManager == null)
                _networkManager = GetComponentInParent<NetworkManager>();
            _serverReqResHandler = new RequestResponseHandler(this);
            _clientReqResHandler = new RequestResponseHandler(this);
        }

        private void Start()
        {
            _networkManager.Subscribe<RequestMessage>(RequestHandler);
            _networkManager.Subscribe<ResponseMessage>(ResponseHandler);
        }

        private void RequestHandler(PlayerID player, RequestMessage msg, bool asServer)
        {
            if (asServer)
                _serverReqResHandler.ProceedRequest(asServer, player, msg);
            else
                _clientReqResHandler.ProceedRequest(asServer, player, msg);
        }

        private void ResponseHandler(PlayerID player, ResponseMessage msg, bool asServer)
        {
            if (asServer)
                _serverReqResHandler.ProceedResponse(player, msg);
            else
                _clientReqResHandler.ProceedResponse(player, msg);
        }

        public bool ServerSendRequest<TRequest>(
            PlayerID playerId,
            ushort requestType,
            TRequest request,
            SerializerDelegate extraRequestSerializer = null,
            ResponseDelegate<object> responseHandler = null,
            int millisecondsTimeout = 0)
            where TRequest : IPacked, new()
        {
            if (millisecondsTimeout <= 0)
                millisecondsTimeout = _serverRequestTimeoutInMilliseconds;
            return _serverReqResHandler.CreateAndSendRequest(playerId, requestType, request, extraRequestSerializer, responseHandler, millisecondsTimeout);
        }

        public async Task<AsyncResponseData<TResponse>> ServerSendRequestAsync<TRequest, TResponse>(
            PlayerID playerId,
            ushort requestType,
            TRequest request,
            SerializerDelegate extraSerializer = null,
            int millisecondsTimeout = 0)
            where TRequest : IPacked, new()
            where TResponse : IPacked, new()
        {
            if (millisecondsTimeout <= 0)
                millisecondsTimeout = _serverRequestTimeoutInMilliseconds;
            bool done = false;
            AsyncResponseData<TResponse> responseData = default;
            // Create and send request
            _serverReqResHandler.CreateAndSendRequest(playerId, requestType, request, extraSerializer, (requestHandler, responseCode, response) =>
            {
                if (!(response is TResponse))
                    response = default(TResponse);
                responseData = new AsyncResponseData<TResponse>(requestHandler, responseCode, (TResponse)response);
                done = true;
            }, millisecondsTimeout);
            // Wait for response
            do { await Task.Delay(100); } while (!done);
            // Return response data
            return responseData;
        }

        public bool ClientSendRequest<TRequest>(
            ushort requestType,
            TRequest request,
            SerializerDelegate extraRequestSerializer = null,
            ResponseDelegate<object> responseHandler = null,
            int millisecondsTimeout = 0)
            where TRequest : IPacked, new()
        {
            if (millisecondsTimeout <= 0)
                millisecondsTimeout = _clientRequestTimeoutInMilliseconds;
            return _clientReqResHandler.CreateAndSendRequest(new PlayerID(new PackedULong(0), false), requestType, request, extraRequestSerializer, responseHandler, millisecondsTimeout);
        }

        public async Task<AsyncResponseData<TResponse>> ClientSendRequestAsync<TRequest, TResponse>(
            ushort requestType,
            TRequest request,
            SerializerDelegate extraSerializer = null,
            int millisecondsTimeout = 0)
            where TRequest : IPacked, new()
            where TResponse : IPacked, new()
        {
            if (millisecondsTimeout <= 0)
                millisecondsTimeout = _clientRequestTimeoutInMilliseconds;
            bool done = false;
            AsyncResponseData<TResponse> responseData = default;
            // Create and send request
            _clientReqResHandler.CreateAndSendRequest(new PlayerID(new PackedULong(0), false), requestType, request, extraSerializer, (requestHandler, responseCode, response) =>
            {
                if (!(response is TResponse))
                    response = default(TResponse);
                responseData = new AsyncResponseData<TResponse>(requestHandler, responseCode, (TResponse)response);
                done = true;
            }, millisecondsTimeout);
            // Wait for response
            do { await Task.Delay(100); } while (!done);
            // Return response data
            return responseData;
        }

        public void RegisterRequestToServer<TRequest, TResponse>(ushort reqType, RequestDelegate<TRequest, TResponse> requestHandler, ResponseDelegate<TResponse> responseHandler = null)
            where TRequest : IPacked, new()
            where TResponse : IPacked, new()
        {
            _serverReqResHandler.RegisterRequestHandler(reqType, requestHandler);
            _clientReqResHandler.RegisterResponseHandler<TRequest, TResponse>(reqType, responseHandler);
        }

        public void UnregisterRequestToServer(ushort reqType)
        {
            _serverReqResHandler.UnregisterRequestHandler(reqType);
            _clientReqResHandler.UnregisterResponseHandler(reqType);
        }

        public void RegisterRequestToClient<TRequest, TResponse>(ushort reqType, RequestDelegate<TRequest, TResponse> requestHandler, ResponseDelegate<TResponse> responseHandler = null)
            where TRequest : IPacked, new()
            where TResponse : IPacked, new()
        {
            _clientReqResHandler.RegisterRequestHandler(reqType, requestHandler);
            _serverReqResHandler.RegisterResponseHandler<TRequest, TResponse>(reqType, responseHandler);
        }

        public void UnregisterRequestToClient(ushort reqType)
        {
            _clientReqResHandler.UnregisterRequestHandler(reqType);
            _serverReqResHandler.UnregisterResponseHandler(reqType);
        }
    }
}
