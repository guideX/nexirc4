/// <summary>
/// Event Aggregator Extensions
/// </summary>
public static class EventAggregatorExtensions {
    /// <summary>
    /// Subscriber on Published Thread
    /// </summary>
    /// <param name="eventAggregator"></param>
    /// <param name="subscriber"></param>
    public static void SubscribeOnPublishedThread(this IEventAggregator eventAggregator, object subscriber) {
        eventAggregator.Subscribe(subscriber, f => f());
    }
    /// <summary>
    /// Subscribe On Background Thread
    /// </summary>
    /// <param name="eventAggregator"></param>
    /// <param name="subscriber"></param>
    public static void SubscribeOnBackgroundThread(this IEventAggregator eventAggregator, object subscriber) {
        eventAggregator.Subscribe(subscriber, f => Task.Factory.StartNew(f, default, TaskCreationOptions.None, TaskScheduler.Default));
    }
    /// <summary>
    /// Subscribe on UI Thread
    /// </summary>
    /// <param name="eventAggregator"></param>
    /// <param name="subscriber"></param>
    public static void SubscribeOnUIThread(this IEventAggregator eventAggregator, object subscriber) {
        eventAggregator.Subscribe(subscriber, f => {
            var taskCompletionSource = new TaskCompletionSource<bool>();
            BeginOnUIThread(async () => {
                try {
                    await f();
                    taskCompletionSource.SetResult(true);
                } catch (OperationCanceledException) {
                    taskCompletionSource.SetCanceled();
                } catch (Exception ex) {
                    taskCompletionSource.SetException(ex);
                }
            });
            return taskCompletionSource.Task;
        });
    }
    /// <summary>
    /// Publish on Current Thread Async
    /// </summary>
    /// <param name="eventAggregator"></param>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static Task PublishOnCurrentThreadAsync(this IEventAggregator eventAggregator, object message, CancellationToken cancellationToken) {
        return eventAggregator.PublishAsync(message, f => f(), cancellationToken);
    }
    /// <summary>
    /// Publish on Current Thread Async
    /// </summary>
    /// <param name="eventAggregator"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static Task PublishOnCurrentThreadAsync(this IEventAggregator eventAggregator, object message) {
        return eventAggregator.PublishOnCurrentThreadAsync(message, default);
    }
    /// <summary>
    /// Publish on Background Thread Async
    /// </summary>
    /// <param name="eventAggregator"></param>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static Task PublishOnBackgroundThreadAsync(this IEventAggregator eventAggregator, object message, CancellationToken cancellationToken) {
        return eventAggregator.PublishAsync(message, f => Task.Factory.StartNew(f, default, TaskCreationOptions.None, TaskScheduler.Default), cancellationToken);
    }
    /// <summary>
    /// Publishes a message on a background thread (async).
    /// </summary>
    /// <param name="eventAggregator">The event aggregator.</param>
    /// <param name = "message">The message instance.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public static Task PublishOnBackgroundThreadAsync(this IEventAggregator eventAggregator, object message) {
        return eventAggregator.PublishOnBackgroundThreadAsync(message, default);
    }
    /// <summary>
    /// Publishes a message on the UI thread.
    /// </summary>
    /// <param name="eventAggregator">The event aggregator.</param>
    /// <param name = "message">The message instance.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public static Task PublishOnUIThreadAsync(this IEventAggregator eventAggregator, object message, CancellationToken cancellationToken) {
        return eventAggregator.PublishAsync(message, f => {
            var taskCompletionSource = new TaskCompletionSource<bool>();

            BeginOnUIThread(async () => {
                try {
                    await f();

                    taskCompletionSource.SetResult(true);
                } catch (OperationCanceledException) {
                    taskCompletionSource.SetCanceled();
                } catch (Exception ex) {
                    taskCompletionSource.SetException(ex);
                }
            });

            return taskCompletionSource.Task;

        }, cancellationToken);
    }
    /// <summary>
    /// Publishes a message on the UI thread.
    /// </summary>
    /// <param name="eventAggregator">The event aggregator.</param>
    /// <param name = "message">The message instance.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public static Task PublishOnUIThreadAsync(this IEventAggregator eventAggregator, object message) {
        return eventAggregator.PublishOnUIThreadAsync(message, default);
    }
    /// <summary>
    /// Begin On UI Thread
    /// </summary>
    /// <param name="action"></param>
    private static void BeginOnUIThread(this Action action) {
        action();
    }
}