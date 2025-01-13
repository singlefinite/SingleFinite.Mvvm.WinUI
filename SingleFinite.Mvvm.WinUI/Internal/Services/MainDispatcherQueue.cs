using System;
using System.Threading.Tasks;
using Microsoft.UI.Dispatching;
using SingleFinite.Mvvm.Services;
using SingleFinite.Mvvm.WinUI.Services;

namespace SingleFinite.Mvvm.WinUI.Internal.Services;

/// <summary>
/// Implementation of IMainDispatcher that uses the
/// <see cref="DispatcherQueue"/> from the main window to execute functions.
/// </summary>
/// <param name="mainWindow">The main window for the app.</param>
/// <param name="exceptionHandler">
/// The exception handler used by this dispatcher to handle exceptions.
/// </param>
internal partial class MainDispatcherQueue(
    IMainWindow mainWindow,
    IExceptionHandler exceptionHandler
) :
    DispatcherBase(exceptionHandler),
    IAppMainDispatcher,
    IDisposable
{
    #region Fields

    /// <summary>
    /// Set to true when this object has been disposed.
    /// </summary>
    private bool _isDisposed = false;

    #endregion

    #region Methods

    /// <summary>
    /// Queue execution of the function on the <see cref="DispatcherQueue"/> set
    /// for this dispatcher.
    /// </summary>
    /// <typeparam name="TResult">
    /// The type of result returned by the function.
    /// </typeparam>
    /// <param name="func">The function to execute.</param>
    /// <returns>A task that runs until the function has completed.</returns>
    public override Task<TResult> RunAsync<TResult>(Func<Task<TResult>> func)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        var dispatcherQueue = mainWindow.Current?.DispatcherQueue ??
            throw new InvalidOperationException("Main window is null.");

        var taskCompletionSource = new TaskCompletionSource<TResult>();

        dispatcherQueue.TryEnqueue(async () =>
        {
            try
            {
                taskCompletionSource.SetResult(await func());
            }
            catch (Exception ex)
            {
                taskCompletionSource.SetException(ex);
            }
        });

        return taskCompletionSource.Task;
    }

    /// <summary>
    /// Mark this object as disposed.
    /// </summary>
    public void Dispose()
    {
        if (_isDisposed)
            return;

        _isDisposed = true;
    }

    #endregion
}
