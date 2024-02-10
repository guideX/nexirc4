using nexIRC.Business.Helper;
using nexIRC.IrcProtocol.Extensions;
using System.Net.Sockets;
namespace nexIRC.IrcProtocol.Connection {
    /// <summary>
    /// Tcp Client Connection
    /// </summary>
    public class TcpClientConnection : IConnection {
        /// <summary>
        /// Host
        /// </summary>
        private readonly string _host;
        /// <summary>
        /// Port
        /// </summary>
        private readonly int _port;
        /// <summary>
        /// Tcp Client
        /// </summary>
        private TcpClient tcpClient;
        /// <summary>
        /// Stream Reader
        /// </summary>
        private StreamReader streamReader;
        /// <summary>
        /// Stream Writer
        /// </summary>
        private StreamWriter streamWriter;
        /// <summary>
        /// Disposed
        /// </summary>
        private bool disposed;
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
        /// Constructor
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public TcpClientConnection(string host, int port) {
            _port = port == 0 ? 6667 : port;
            _host = host;
            tcpClient = new TcpClient();
            streamReader = StreamReader.Null;
            streamWriter = StreamWriter.Null;
            try {
                if (string.IsNullOrWhiteSpace(host)) throw new ArgumentNullException(nameof(host));
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.IrcProtocol.TcpClientConnection");
            }
        }
        /// <summary>
        /// Reset Tcp Client
        /// </summary>
        private void ResetTcpClient() {
            try {
                tcpClient?.Dispose();
                tcpClient = new TcpClient();
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "ResetTcpClient");
            }
        }
        /// <summary>
        /// Connect Async
        /// </summary>
        /// <returns></returns>
        public async Task ConnectAsync() {
            try {
                ResetTcpClient();
                await tcpClient.ConnectAsync(_host, _port).ConfigureAwait(false);
                streamReader = new StreamReader(tcpClient.GetStream());
                streamWriter = new StreamWriter(tcpClient.GetStream());
                Connected?.Invoke(this, EventArgs.Empty);
                RunDataReceiver().SafeFireAndForget(continueOnCapturedContext: false, onException: ex => Disconnected?.Invoke(this, EventArgs.Empty));
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.IrcProtocol.ConnectAsync");
            }
        }
        /// <summary>
        /// Run Data Receiver
        /// </summary>
        /// <returns></returns>
        private async Task RunDataReceiver() {
            try {
                string? line;
                while ((line = await streamReader.ReadLineAsync().ConfigureAwait(false)) != null) {
                    DataReceived?.Invoke(this, new DataReceivedEventArgs(line));
                }
                Disconnected?.Invoke(this, EventArgs.Empty);
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.IrcProtocol.RunDataReceiver");
            }
        }
        /// <summary>
        /// Send Async
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task SendAsync(string data) {
            try {
                if (!data.EndsWith(ConstantsHelper.CrLf)) data += ConstantsHelper.CrLf;
                await streamWriter.WriteAsync(data).ConfigureAwait(false);
                await streamWriter.FlushAsync().ConfigureAwait(false);
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.IrcProtocol.SendAsync");
            }
        }
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing) {
            if (disposed) return;
            if (disposing) {
                streamReader?.Dispose();
                streamWriter?.Dispose();
                tcpClient!.Dispose();
            }
            disposed = true;
        }
        ~TcpClientConnection() => Dispose(false);
    }
}