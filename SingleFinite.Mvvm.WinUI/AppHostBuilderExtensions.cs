﻿// MIT License
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
using SingleFinite.Mvvm.Services;
using SingleFinite.Mvvm.WinUI.Internal.Services;
using SingleFinite.Mvvm.WinUI.Services;

namespace SingleFinite.Mvvm.WinUI;

/// <summary>
/// Extensions for the <see cref="AppHostBuilder"/> class.
/// </summary>
public static class AppHostBuilderExtensions
{
    #region Methods

    /// <summary>
    /// Add WinUI services to the app.
    /// </summary>
    /// <typeparam name="TMainViewModel">
    /// The type of view model that will be built as the entry point for app.
    /// The view registered for the view model must be of type 
    /// <see cref="Microsoft.UI.Xaml.Window"/>
    /// </typeparam>
    /// <param name="builder">The builder to extend.</param>
    /// <returns>The builder that was passed in.</returns>
    public static AppHostBuilder AddWinUI<TMainViewModel>(
        this AppHostBuilder builder
    )
        where TMainViewModel : IViewModel => builder
            .AddServices(
                services =>
                {
                    services
                        .AddSingleton<MainWindow>()
                        .AddSingleton<IMainWindow>(serviceProvider =>
                        {
                            return serviceProvider.GetRequiredService<MainWindow>();
                        })
                        .AddSingleton<IMainWindowProvider>(serviceProvider =>
                        {
                            return serviceProvider.GetRequiredService<MainWindow>();
                        })
                        .AddSingleton<IApplicationMainDispatcher, WinUIMainDispatcher>()
                        .AddSingleton<IWinUIApp, WinUIApp<TMainViewModel>>()
                        .AddScoped<IMainDispatcher, WinUIMainDispatcher>();
                }
            )
            .AddOnStarted<IWinUIApp>(
                app => app.Start()
            );

    #endregion
}
