// MIT License
// Copyright (c) 2024 Single Finite
//
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights 
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
// copies of the Software, and to permit persons to whom the Software is 
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in 
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Diagnostics;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using SingleFinite.Essentials;
using SingleFinite.Mvvm.Services;
using SingleFinite.Mvvm.WinUI.Services;
using Windows.Foundation;

namespace SingleFinite.Mvvm.WinUI.Internal.Services;

/// <summary>
/// Implementation of <see cref="IWinUIApp"/>.
/// </summary>
/// <param name="appHost">Listen for closed event.</param>
/// <param name="mainWindow">Hold the main window for the app.</param>
/// <param name="viewBuilder">Used to build HostWindow view.</param>
/// <param name="mainDispatcher">The main dispatcher.</param>
/// <param name="exceptionHandler">Used to log unhandled exceptions.</param>
/// <param name="cancellationTokenProvider">
/// Used to unsubscribe observers.
/// </param>
/// <typeparam name="TMainViewModel">
/// The type of view model to build for the HostWindow.
/// </typeparam>
internal partial class WinUIApp<TMainViewModel>(
    IAppHost appHost,
    IMainWindowProvider mainWindow,
    IViewBuilder viewBuilder,
    IMainDispatcher mainDispatcher,
    IExceptionHandler exceptionHandler,
    ICancellationTokenProvider cancellationTokenProvider
) :
    IWinUIApp,
    IDisposable
    where TMainViewModel : IViewModel
{
    #region Fields

    /// <summary>
    /// Set to true when the app has been disposed.
    /// </summary>
    private bool _isDisposed;

    #endregion

    #region Methods

    /// <inheritdoc/>
    public void Start()
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        if (mainWindow.Current is not null)
            return;

        var hostWindow = viewBuilder.Build<TMainViewModel>() as Window ??
            throw new InvalidOperationException(
                "The view for the host view model must be a Window."
            );

        mainWindow.Current = hostWindow;

        Application.Current.UnhandledException += (sender, e) =>
        {
            exceptionHandler.Handle(e.Exception);
            e.Handled = true;
        };

        appHost.Closed
            .Observe()
            .OnEach(hostWindow.Close)
            .On(cancellationTokenProvider.CancellationToken);

        Observable
            .Observe<TypedEventHandler<AppWindow, AppWindowClosingEventArgs>>(
                register: handler => hostWindow.AppWindow.Closing += handler,
                unregister: handler => hostWindow.AppWindow.Closing -= handler,
                handler: _ => (_, args) =>
                {
                    args.Cancel = true;
                    mainDispatcher.Run(appHost.CloseAsync);
                }
            )
            .On(cancellationTokenProvider.CancellationToken);
    }

    /// <inheritdoc/>
    public void Activate()
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        // This is a workaround for when the Activate method doesn't bring the
        // main window to the foreground.
        //
        var processId = AppInstance.GetCurrent().ProcessId;
        var process = Process.GetProcessById((int)processId);
        Win32.SetForegroundWindow(process.MainWindowHandle);

        mainWindow.Current?.Activate();
    }

    /// <summary>
    /// Mark this object as disposed.
    /// </summary>
    public void Dispose()
    {
        _isDisposed = true;
    }

    #endregion
}
