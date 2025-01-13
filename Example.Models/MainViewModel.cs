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
using SingleFinite.Mvvm;
using SingleFinite.Mvvm.Services;

namespace SingleFinite.Example.Models;

/// <summary>
/// The host view model.
/// </summary>
/// <param name="appHost">The app host.</param>
/// <param name="eventObserver">Event observer.</param>
/// <param name="dialogs">Dialog presenter.</param>
/// <param name="content">Content presenter.</param>
public partial class HostViewModel(
    IAppHost appHost,
    IEventObserver eventObserver,
    IDialogs dialogs,
    IPresentableItem content
) : ViewModel
{
    #region Properties

    /// <summary>
    /// Dialog presenter.
    /// </summary>
    public IDialogs Dialogs => dialogs;

    /// <summary>
    /// Content presenter.
    /// </summary>
    public IPresentableItem Content => content;

    #endregion

    #region Methods

    protected override void OnInitialize()
    {
        eventObserver
            .Observe(appHost.Closing)
            .OnEach(async args =>
            {
                var dialog = await dialogs.ShowAsync<MessageDialogViewModel, MessageDialogViewModel.Context>(
                    new(
                        Title: "Close App",
                        Message: "Do you want to close the app?",
                        PrimaryText: "Yes",
                        CancelText: "No"
                    )
                );

                args.Cancel = dialog.Result == MessageDialogViewModel.MessageResult.Cancel;
            });

        content.Set<NavigatorViewModel>();
    }

    #endregion
}
