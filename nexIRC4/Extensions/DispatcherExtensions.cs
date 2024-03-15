using System;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
namespace nexIRC.Extensions {
    /// <summary>
    /// Dispatcher Extensions
    /// </summary>
    public static class DispatcherExtensions {
        /// <summary>
        /// Switch to UI
        /// </summary>
        /// <param name="dispatcher"></param>
        /// <returns></returns>
        public static SwitchToUiAwaitable SwitchToUi(this Dispatcher dispatcher) {
            return new SwitchToUiAwaitable(dispatcher);
        }
        /// <summary>
        /// Switch To UI Awaitable
        /// </summary>
        public struct SwitchToUiAwaitable : INotifyCompletion {
            /// <summary>
            /// Dispatcher
            /// </summary>
            private readonly Dispatcher _dispatcher;
            /// <summary>
            /// Switch To UI Awaitable
            /// </summary>
            /// <param name="dispatcher"></param>
            public SwitchToUiAwaitable(Dispatcher dispatcher) {
                _dispatcher = dispatcher;
            }
            /// <summary>
            /// Get Awaiter
            /// </summary>
            /// <returns></returns>
            public SwitchToUiAwaitable GetAwaiter() {
                return this;
            }
            /// <summary>
            /// Get Result
            /// </summary>
            public void GetResult() {
            }
            /// <summary>
            /// Is Completed
            /// </summary>
            public bool IsCompleted => _dispatcher.CheckAccess();
            /// <summary>
            /// On Completed
            /// </summary>
            /// <param name="continuation"></param>
            public void OnCompleted(Action continuation) {
                _dispatcher.BeginInvoke(continuation);
            }
        }
    }
}