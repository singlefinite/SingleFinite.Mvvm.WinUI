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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using SingleFinite.Example.App.Views;
using SingleFinite.Example.Models;
using SingleFinite.Mvvm;
using SingleFinite.Mvvm.Services;
using SingleFinite.Mvvm.WinUI;
using SingleFinite.Mvvm.WinUI.Services;

namespace SingleFinite.Example.App;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    #region Fields

    /// <summary>
    /// Holds the app host.
    /// </summary>
    private IAppHost? _appHost = null;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes the singleton application object.  This is the first line of
    /// authored code executed, and as such is the logical equivalent of main()
    /// or WinMain().
    /// </summary>
    public App()
    {
        InitializeComponent();
    }

    #endregion

    #region Methods

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        _appHost ??= new AppHostBuilder()
            .AddWinUI<HostViewModel>()
            .AddExampleViews()
            .BuildAndStart();

        _appHost.ServiceProvider.GetRequiredService<IWinUIApp>().Activate();
    }

    #endregion
}
