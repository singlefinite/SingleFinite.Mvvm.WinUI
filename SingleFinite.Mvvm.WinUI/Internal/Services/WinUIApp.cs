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
using System.Runtime.InteropServices;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
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
/// <param name="eventObserver">Used to observe events.</param>
/// <param name="mainDispatcher">The main dispatcher.</param>
/// <param name="exceptionHandler">Used to log unhandled exceptions.</param>
/// <typeparam name="THostViewModel">
/// The type of view model to build for the HostWindow.
/// </typeparam>
internal partial class WinUIApp<THostViewModel>(
    IAppHost appHost,
    IMainWindowProvider mainWindow,
    IViewBuilder viewBuilder,
    IEventObserver eventObserver,
    IMainDispatcher mainDispatcher,
    IExceptionHandler exceptionHandler
) :
    IWinUIApp,
    IDisposable
    where THostViewModel : IViewModel
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

        mainWindow.Current = viewBuilder.Build<THostViewModel>() as Window ??
            throw new InvalidOperationException(
                "The view for the host view model must be a Window."
            );

        Application.Current.UnhandledException += (sender, e) =>
        {
            e.Handled = exceptionHandler.Handle(e.Exception);
        };

        eventObserver.Observe(
            observable: appHost.Closed,
            callback: () => mainWindow.Current.Close()
        );

        eventObserver.Observe<TypedEventHandler<AppWindow, AppWindowClosingEventArgs>>(
            register: handler => mainWindow.Current.AppWindow.Closing += handler,
            unregister: handler => mainWindow.Current.AppWindow.Closing -= handler,
            callback: (_, args) =>
            {
                args.Cancel = true;
                mainDispatcher.Run(appHost.CloseAsync);
            }
        );
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
