/// <summary>
/// Event Aggregator
/// </summary>
public interface IEventAggregator {
    /// <summary>
    /// Handlers Exist For
    /// </summary>
    /// <param name="messageType"></param>
    /// <returns></returns>
    bool HandlerExistsFor(Type messageType);
    /// <summary>
    /// Subscribe
    /// </summary>
    /// <param name="subscriber"></param>
    /// <param name="marshal"></param>
    void Subscribe(object subscriber, Func<Func<Task>, Task> marshal);
    /// <summary>
    /// Unsubscribe
    /// </summary>
    /// <param name="subscriber"></param>
    void Unsubscribe(object subscriber);
    /// <summary>
    /// Publish Async
    /// </summary>
    /// <param name="message"></param>
    /// <param name="marshal"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task PublishAsync(object message, Func<Func<Task>, Task> marshal, CancellationToken cancellationToken = default);
}