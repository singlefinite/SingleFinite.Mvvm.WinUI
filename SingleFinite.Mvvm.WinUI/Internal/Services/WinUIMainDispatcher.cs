using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.UI.Dispatching;
using SingleFinite.Essentials;
using SingleFinite.Mvvm.Services;
using SingleFinite.Mvvm.WinUI.Services;

namespace SingleFinite.Mvvm.WinUI.Internal.Services;

/// <summary>
/// Implementation of IMainDispatcher that uses the
/// <see cref="DispatcherQueue"/> from the main window to execute functions.
/// </summary>
internal sealed partial class WinUIMainDispatcher :
    IApplicationMainDispatcher,
    IDisposable
{
    #region Fields

    /// <summary>
    /// Holds the dispose state for this object.
    /// </summary>
    private readonly DisposeState _disposeState;

    /// <summary>
    /// Holds the main window.
    /// </summary>
    private readonly IMainWindow _mainWindow;

    /// <summary>
    /// Holds the exception handler.
    /// </summary>
    private readonly IExceptionHandler _exceptionHandler;

    #region Properties

    /// <inheritdoc/>
    public CancellationToken CancellationToken => throw new NotImplementedException();

    #endregion

    #endregion

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="mainWindow">The main window for the app.</param>
    /// <param name="exceptionHandler">
    /// The exception handler used by this dispatcher to handle exceptions.
    /// </param>
    public WinUIMainDispatcher(
        IMainWindow mainWindow,
        IExceptionHandler exceptionHandler
    )
    {
        _mainWindow = mainWindow;
        _exceptionHandler = exceptionHandler;

        _disposeState = new DisposeState(owner: this);
    }

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
    public Task<TResult> RunAsync<TResult>(Func<Task<TResult>> func)
    {
        _disposeState.ThrowIfDisposed();

        var dispatcherQueue = _mainWindow.Current?.DispatcherQueue ??
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
    /// Use ExceptionHandler to handle exception.
    /// </summary>
    /// <param name="ex">The exception to handle.</param>
    public void OnError(Exception ex) => _exceptionHandler.Handle(ex);

    /// <inheritdoc/>
    public void Dispose() => _disposeState.Dispose();

    #endregion
}
