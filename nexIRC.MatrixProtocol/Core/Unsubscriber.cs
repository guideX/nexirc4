namespace nexIRC.MatrixProtocol.Core {
    /// <summary>
    /// Unsubscriber
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class Unsubscriber<T> : IDisposable {
        /// <summary>
        /// Observer
        /// </summary>
        private readonly IObserver<T> _observer;
        /// <summary>
        /// Observers
        /// </summary>
        private readonly List<IObserver<T>> _observers;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="observers"></param>
        /// <param name="observer"></param>
        public Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer) {
            _observers = observers;
            _observer = observer;
        }
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() {
            if (_observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }
}