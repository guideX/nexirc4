using System.Reflection;
/// <summary>
/// Event Aggregator
/// </summary>
public class EventAggregator : IEventAggregator {
    /// <summary>
    /// Handlers
    /// </summary>
    private readonly List<Handler> _handlers = new List<Handler>();
    /// <summary>
    /// Handlers Exist For
    /// </summary>
    /// <param name="messageType"></param>
    /// <returns></returns>
    public virtual bool HandlerExistsFor(Type messageType) {
        lock (_handlers)
            return _handlers.Any(handler => handler.Handles(messageType) && !handler.IsDead);
    }
    /// <summary>
    /// Subscribe
    /// </summary>
    /// <param name="subscriber"></param>
    /// <param name="marshal"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public virtual void Subscribe(object subscriber, Func<Func<Task>, Task> marshal) {
        if (subscriber == null) throw new ArgumentNullException(nameof(subscriber));
        if (marshal == null) throw new ArgumentNullException(nameof(marshal));
        lock (_handlers) {
            if (_handlers.Any(x => x.Matches(subscriber))) return;
            _handlers.Add(new Handler(subscriber, marshal));
        }
    }
    /// <summary>
    /// Unsubscribe
    /// </summary>
    /// <param name="subscriber"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public virtual void Unsubscribe(object subscriber) {
        if (subscriber == null) throw new ArgumentNullException(nameof(subscriber));
        lock (_handlers) {
            var found = _handlers.FirstOrDefault(x => x.Matches(subscriber));
            if (found != null) _handlers.Remove(found);
        }
    }
    /// <summary>
    /// Publish Async
    /// </summary>
    /// <param name="message"></param>
    /// <param name="marshal"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public virtual Task PublishAsync(object message, Func<Func<Task>, Task> marshal, CancellationToken cancellationToken = default) {
        if (message == null) throw new ArgumentNullException(nameof(message));
        if (marshal == null) throw new ArgumentNullException(nameof(marshal));
        Handler[] toNotify;
        lock (_handlers) toNotify = _handlers.ToArray();
        return marshal(async () => {
            var messageType = message.GetType();
            var tasks = toNotify.Select(h => h.Handle(messageType, message, CancellationToken.None));
            await Task.WhenAll(tasks);
            var dead = toNotify.Where(h => h.IsDead).ToList();
            if (dead.Any()) {
                lock (_handlers) {
                    foreach (var d in dead) {
                        _handlers.Remove(d);
                    }
                }
            }
        });
    }
    /// <summary>
    /// Handler
    /// </summary>
    private class Handler {
        /// <summary>
        /// Marshal
        /// </summary>
        private readonly Func<Func<Task>, Task> _marshal;
        /// <summary>
        /// Reference
        /// </summary>
        private readonly WeakReference _reference;
        /// <summary>
        /// Dictionary
        /// </summary>
        private readonly Dictionary<Type, MethodInfo> _supportedHandlers = new Dictionary<Type, MethodInfo>();
        /// <summary>
        /// Handler
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="marshal"></param>
        public Handler(object handler, Func<Func<Task>, Task> marshal) {
            _marshal = marshal;
            _reference = new WeakReference(handler);
            var interfaces = handler.GetType().GetTypeInfo().ImplementedInterfaces
                .Where(x => x.GetTypeInfo().IsGenericType && x.GetGenericTypeDefinition() == typeof(IHandle<>));
            foreach (var @interface in interfaces) {
                var type = @interface.GetTypeInfo().GenericTypeArguments[0];
                var method = @interface.GetRuntimeMethod("HandleAsync", new[] { type, typeof(CancellationToken) });
                if (method != null) _supportedHandlers[type] = method;
            }
        }
        /// <summary>
        /// Is Dead
        /// </summary>
        public bool IsDead => _reference.Target == null;
        /// <summary>
        /// Matches
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public bool Matches(object instance) {
            return _reference.Target == instance;
        }
        /// <summary>
        /// Handle
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task Handle(Type messageType, object message, CancellationToken cancellationToken) {
            var target = _reference.Target;
            if (target == null) return Task.FromResult(false);
            return _marshal(() => {
                var tasks = _supportedHandlers
                    .Where(handler => handler.Key.GetTypeInfo().IsAssignableFrom(messageType.GetTypeInfo()))
                    .Select(pair => pair.Value.Invoke(target, new[] { message, cancellationToken }))
                    .Select(result => (Task)result!)
                    .ToList();

                return Task.WhenAll(tasks!);
            });
        }
        /// <summary>
        /// Handles
        /// </summary>
        /// <param name="messageType"></param>
        /// <returns></returns>
        public bool Handles(Type messageType) {
            return _supportedHandlers.Any(pair => pair.Key.GetTypeInfo().IsAssignableFrom(messageType.GetTypeInfo()));
        }
    }
}