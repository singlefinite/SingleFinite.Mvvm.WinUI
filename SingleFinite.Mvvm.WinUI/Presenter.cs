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
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SingleFinite.Mvvm.Services;

namespace SingleFinite.Mvvm.WinUI;

/// <summary>
/// Presents views in a simple control.
/// </summary>
public sealed partial class Presenter : ContentControl
{
    #region Fields

    /// <summary>
    /// Observer for view changes on the source presentable.
    /// </summary>
    private IDisposable? _sourceViewObserver;

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor.
    /// </summary>
    public Presenter()
    {
        DefaultStyleKey = typeof(Presenter);
        Loaded += (_, _) => Subscribe();
        Unloaded += (_, _) => Unsubscribe();
    }

    #endregion

    #region Properties

    /// <summary>
    /// The source presentable whose views will be displayed in this control.
    /// </summary>
    public IPresentable? Source
    {
        get;
        set
        {
            if (field == value)
                return;

            Unsubscribe();

            field = value;

            Subscribe();
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Observe changes to the current view of the presentable.
    /// </summary>
    private void Subscribe()
    {
        if (_sourceViewObserver is not null)
            return;

        _sourceViewObserver = Source?.CurrentChanged?.Observe(
            args => UpdateContent(args.View)
        );

        UpdateContent(Source?.Current);
    }

    /// <summary>
    /// Stop observing changes to the current view of the presentable.
    /// </summary>
    private void Unsubscribe()
    {
        _sourceViewObserver?.Dispose();
        _sourceViewObserver = null;
    }

    /// <summary>
    /// Update the view that is being displayed.
    /// </summary>
    /// <param name="view">The view to display.</param>
    private void UpdateContent(IView? view)
    {
        Content = view as UIElement;
    }

    #endregion
}
