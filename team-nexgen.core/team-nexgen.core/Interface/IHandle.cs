/// <summary>
/// Denotes a class which can handle a particular type of message.
/// Source code from Caliburn.Micro
/// https://github.com/Caliburn-Micro/Caliburn.Micro/blob/master/src/Caliburn.Micro.Core/EventAggregator.cs
/// </summary>
/// <typeparam name = "TMessage">The type of message to handle.</typeparam>
// ReSharper disable once TypeParameterCanBeVariant
public interface IHandle<TMessage> {
    /// <summary>
    /// Handles the message.
    /// </summary>
    /// <param name = "message">The message.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A task that represents the asynchronous coroutine.</returns>
    Task HandleAsync(TMessage message, CancellationToken cancellationToken);
}