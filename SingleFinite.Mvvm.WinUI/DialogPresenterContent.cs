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

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace SingleFinite.Mvvm.WinUI;

/// <summary>
/// Display content in a dialog.
/// </summary>
public sealed partial class DialogPresenterContent : ContentControl
{
    #region Constructors

    /// <summary>
    /// Constructor.
    /// </summary>
    public DialogPresenterContent()
    {
        DefaultStyleKey = typeof(DialogPresenterContent);
    }

    #endregion

    /// <summary>
    /// Dependency property for TopBar.
    /// </summary>
    public static readonly DependencyProperty TopBarProperty = DependencyProperty.Register(
        name: nameof(TopBar),
        propertyType: typeof(object),
        ownerType: typeof(DialogPresenterContent),
        typeMetadata: new(defaultValue: null)
    );

    /// <summary>
    /// The area at the top of the control.
    /// </summary>
    public object? TopBar
    {
        get => GetValue(TopBarProperty);
        set => SetValue(TopBarProperty, value);
    }

    /// <summary>
    /// Dependency property for TopBarMargin.
    /// </summary>
    public static readonly DependencyProperty TopBarMarginProperty = DependencyProperty.Register(
        name: nameof(TopBarMargin),
        propertyType: typeof(Thickness),
        ownerType: typeof(DialogPresenterContent),
        typeMetadata: new(defaultValue: new Thickness())
    );

    /// <summary>
    /// The margin for the top bar.
    /// </summary>
    public Thickness TopBarMargin
    {
        get => (Thickness)GetValue(TopBarMarginProperty);
        set => SetValue(TopBarMarginProperty, value);
    }

    /// <summary>
    /// Dependency property for BottomBar.
    /// </summary>
    public static readonly DependencyProperty BottomBarProperty = DependencyProperty.Register(
        name: nameof(BottomBar),
        propertyType: typeof(object),
        ownerType: typeof(DialogPresenterContent),
        typeMetadata: new(defaultValue: null)
    );

    /// <summary>
    /// The area at the bottom of the control.
    /// </summary>
    public object? BottomBar
    {
        get => GetValue(BottomBarProperty);
        set => SetValue(BottomBarProperty, value);
    }

    /// <summary>
    /// Dependency property for BottomBarPadding.
    /// </summary>
    public static readonly DependencyProperty BottomBarPaddingProperty = DependencyProperty.Register(
        name: nameof(BottomBarPadding),
        propertyType: typeof(Thickness),
        ownerType: typeof(DialogPresenterContent),
        typeMetadata: new(defaultValue: new Thickness())
    );

    /// <summary>
    /// The padding for the bottom bar.
    /// </summary>
    public Thickness BottomBarPadding
    {
        get => (Thickness)GetValue(BottomBarPaddingProperty);
        set => SetValue(BottomBarPaddingProperty, value);
    }
}
