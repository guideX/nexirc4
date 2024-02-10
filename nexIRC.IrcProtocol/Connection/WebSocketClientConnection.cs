using nexIRC.Business.Helper;
using nexIRC.IrcProtocol.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Net.WebSockets;
using System.Text;
namespace nexIRC.IrcProtocol.Connection {
    /// <summary>
    /// Web Socket Client Connection
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class WebSocketClientConnection : IConnection {
        /// <summary>
        /// Client Web Socket
        /// </summary>
        private readonly ClientWebSocket clientWebSocket = new ClientWebSocket();
        /// <summary>
        /// Disposal Token Source
        /// </summary>
        private readonly CancellationTokenSource disposalTokenSource = new CancellationTokenSource();
        /// <summary>
        /// Data Received
        /// </summary>
        public event EventHandler<DataReceivedEventArgs>? DataReceived;
        /// <summary>
        /// Connected
        /// </summary>
        public event EventHandler? Connected;
        /// <summary>
        /// Disconnected
        /// </summary>
        public event EventHandler? Disconnected;
        /// <summary>
        /// Address
        /// </summary>
        private readonly string _address;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="address"></param>
        public WebSocketClientConnection(string address) {
            _address = address;
        }
        /// <summary>
        /// Connect Async
        /// </summary>
        /// <returns></returns>
        public async Task ConnectAsync() {
            try {
                await clientWebSocket.ConnectAsync(new Uri(_address), disposalTokenSource.Token).ConfigureAwait(false);
                while (clientWebSocket.State != WebSocketState.Open) {
                    await Task.Delay(100).ConfigureAwait(false);
                }
                Connected?.Invoke(this, EventArgs.Empty);
                RunDataReceiver().SafeFireAndForget(continueOnCapturedContext: false, onException: ex => Disconnected?.Invoke(this, EventArgs.Empty));
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.IrcProtocol.Connection.ConnectAsync");
            }
        }
        /// <summary>
        /// Run Data Receiver
        /// </summary>
        /// <returns></returns>
        private async Task RunDataReceiver() {
            var buffer = new ArraySegment<byte>(new byte[1024]);
            while (!disposalTokenSource.IsCancellationRequested) {
                var received = await clientWebSocket.ReceiveAsync(buffer, disposalTokenSource.Token).ConfigureAwait(false);
                var receivedAsText = Encoding.ASCII.GetString(buffer.Array!, 0, received.Count);
                DataReceived?.Invoke(this, new DataReceivedEventArgs(receivedAsText));
            }
            Disconnected?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// Send Async
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task SendAsync(string data) {
            if (!data.EndsWith(ConstantsHelper.CrLf)) data += ConstantsHelper.CrLf;
            var dataToSend = new ArraySegment<byte>(Encoding.ASCII.GetBytes(data));
            await clientWebSocket.SendAsync(dataToSend, WebSocketMessageType.Text, true, disposalTokenSource.Token).ConfigureAwait(false);
        }
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() {
            disposalTokenSource.Cancel();
            _ = clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
        }
    }
}