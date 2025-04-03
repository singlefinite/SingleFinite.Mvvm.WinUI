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
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using SingleFinite.Essentials;
using SingleFinite.Mvvm.Services;
using SingleFinite.Mvvm.WinUI.Internal;

namespace SingleFinite.Mvvm.WinUI;

/// <summary>
/// Presents views in a Frame control.
/// </summary>
public sealed partial class FramePresenter : Control
{
    #region Fields

    /// <summary>
    /// Observer for view changes on the source presentable.
    /// </summary>
    private IDisposable? _sourceViewObserver;

    /// <summary>
    /// The frame control used to display views.
    /// </summary>
    private readonly Frame _frame = new();

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor.
    /// </summary>
    public FramePresenter()
    {
        DefaultStyleKey = typeof(FramePresenter);
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

    /// <summary>
    /// The default transition to apply when navigating forward.
    /// If null a default transition will be used.
    /// Default is null.
    /// </summary>
    public NavigationTransitionInfo? DefaultForwardTransition { get; set; }

    /// <summary>
    /// The default transition to apply when navigating backward.
    /// If null a default transition will be used.
    /// Default is null.
    /// </summary>
    public NavigationTransitionInfo? DefaultBackwardTransition { get; set; }

    #endregion

    #region Methods

    /// <summary>
    /// Display the frame control in the ContentPresenter defined in the
    /// template.
    /// </summary>
    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        var dialogContent = GetTemplateChild("ViewContent");
        if (dialogContent is ContentPresenter contentPresenter)
            contentPresenter.Content = _frame;
    }

    /// <summary>
    /// Observe changes to the current view of the presentable.
    /// </summary>
    private void Subscribe()
    {
        if (_sourceViewObserver is not null)
            return;

        _sourceViewObserver =
            Source?.CurrentChanged
                ?.Observe()
                ?.OnEach(
                    args => UpdateContent(
                        view: args.View,
                        isNew: args.IsNew
                    )
                );

        UpdateContent(
            view: Source?.Current,
            isNew: true
        );
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
    /// <param name="isNew">
    /// True if the view is new and we should use the forward animation.
    /// False if the view is not new and we sould use the backward animation.
    /// </param>
    private void UpdateContent(IView? view, bool isNew)
    {
        _frame.BackStack.Clear();
        _frame.ForwardStack.Clear();

        if (isNew)
        {
            var transition =
                (view as IFramePresenterPage)?.ForwardTransition ??
                DefaultForwardTransition;

            _frame.Navigate(
                sourcePageType: typeof(PresenterPage),
                parameter: view,
                infoOverride: transition
            );
        }
        else
        {
            var transition =
                (view as IFramePresenterPage)?.BackwardTransition ??
                DefaultBackwardTransition;

            _frame.BackStack.Add(
                new(
                    sourcePageType: typeof(PresenterPage),
                    parameter: view,
                    navigationTransitionInfo: null
                )
            );
            _frame.GoBack(
                transitionInfoOverride: transition
            );
        }
    }

    #endregion
}
