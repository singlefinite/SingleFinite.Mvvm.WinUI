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

using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace SingleFinite.Mvvm.WinUI;

/// <summary>
/// Control that displays buttons in a dialog.
/// </summary>
public sealed partial class DialogPresenterButtons : Control
{
    #region Fields

    /// <summary>
    /// Holds reference to the primary button.
    /// </summary>
    private Button? _primaryButton;

    /// <summary>
    /// Holds reference to the secondary button.
    /// </summary>
    private Button? _secondaryButton;

    /// <summary>
    /// Holds reference to the close button.
    /// </summary>
    private Button? _closeButton;

    /// <summary>
    /// Holds reference to the first column in the grid layout.
    /// </summary>
    private ColumnDefinition? _firstColumn;

    /// <summary>
    /// Holds reference to the first spacer in the grid layout.
    /// </summary>
    private ColumnDefinition? _firstSpacer;

    /// <summary>
    /// Holds reference to the second column in the grid layout.
    /// </summary>
    private ColumnDefinition? _secondColumn;

    /// <summary>
    /// Holds reference to the second spacer in the grid layout.
    /// </summary>
    private ColumnDefinition? _secondSpacer;

    /// <summary>
    /// Holds reference to the third column in the grid layout.
    /// </summary>
    private ColumnDefinition? _thirdColumn;

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor.
    /// </summary>
    public DialogPresenterButtons()
    {
        DefaultStyleKey = typeof(DialogPresenterButtons);
    }

    #endregion

    #region Properties

    /// <summary>
    /// Dependency property for PrimaryButtonStyle.
    /// </summary>
    public static readonly DependencyProperty PrimaryButtonStyleProperty = DependencyProperty.Register(
        name: nameof(PrimaryButtonStyle),
        propertyType: typeof(Style),
        ownerType: typeof(DialogPresenterButtons),
        typeMetadata: new(defaultValue: new Style())
    );

    /// <summary>
    /// The style to apply to the primary button.
    /// </summary>
    public Style PrimaryButtonStyle
    {
        get => (Style)GetValue(PrimaryButtonStyleProperty);
        set => SetValue(PrimaryButtonStyleProperty, value);
    }

    /// <summary>
    /// Dependency property for PrimaryButtonText.
    /// </summary>
    public static readonly DependencyProperty PrimaryButtonTextProperty = DependencyProperty.Register(
        name: nameof(PrimaryButtonText),
        propertyType: typeof(string),
        ownerType: typeof(DialogPresenterButtons),
        typeMetadata: new(
            defaultValue: null,
            propertyChangedCallback: (sender, _) => UpdateButtonLayout(sender)
        )
    );

    /// <summary>
    /// The text for the primary button.
    /// If the value is null or an empty string the button will not be visible.
    /// </summary>
    public string? PrimaryButtonText
    {
        get => (string)GetValue(PrimaryButtonTextProperty);
        set => SetValue(PrimaryButtonTextProperty, value);
    }

    /// <summary>
    /// Dependency property for IsPrimaryButtonEnabled.
    /// </summary>
    public static readonly DependencyProperty IsPrimaryButtonEnabledProperty = DependencyProperty.Register(
        name: nameof(IsPrimaryButtonEnabled),
        propertyType: typeof(bool),
        ownerType: typeof(DialogPresenterButtons),
        typeMetadata: new(defaultValue: true)
    );

    /// <summary>
    /// Indicates if the primary button is enabled.
    /// </summary>
    public bool IsPrimaryButtonEnabled
    {
        get => (bool)GetValue(IsPrimaryButtonEnabledProperty);
        set => SetValue(IsPrimaryButtonEnabledProperty, value);
    }

    /// <summary>
    /// Dependency property for SecondaryButtonStyle.
    /// </summary>
    public static readonly DependencyProperty SecondaryButtonStyleProperty = DependencyProperty.Register(
        name: nameof(SecondaryButtonStyle),
        propertyType: typeof(Style),
        ownerType: typeof(DialogPresenterButtons),
        typeMetadata: new(defaultValue: new Style())
    );

    /// <summary>
    /// The style to apply to the secondary button.
    /// </summary>
    public Style SecondaryButtonStyle
    {
        get => (Style)GetValue(SecondaryButtonStyleProperty);
        set => SetValue(SecondaryButtonStyleProperty, value);
    }

    /// <summary>
    /// Dependency property for SecondaryButtonText.
    /// </summary>
    public static readonly DependencyProperty SecondaryButtonTextProperty = DependencyProperty.Register(
        name: nameof(SecondaryButtonText),
        propertyType: typeof(string),
        ownerType: typeof(DialogPresenterButtons),
        typeMetadata: new(
            defaultValue: null,
            propertyChangedCallback: (sender, _) => UpdateButtonLayout(sender)
        )
    );

    /// <summary>
    /// The text for the secondary button.
    /// If the value is null or an empty string the button will not be visible.
    /// </summary>
    public string? SecondaryButtonText
    {
        get => (string)GetValue(SecondaryButtonTextProperty);
        set => SetValue(SecondaryButtonTextProperty, value);
    }

    /// <summary>
    /// Dependency property for IsSecondaryButtonEnabled.
    /// </summary>
    public static readonly DependencyProperty IsSecondaryButtonEnabledProperty = DependencyProperty.Register(
        name: nameof(IsSecondaryButtonEnabled),
        propertyType: typeof(bool),
        ownerType: typeof(DialogPresenterButtons),
        typeMetadata: new(defaultValue: true)
    );

    /// <summary>
    /// Indicates if the secondary button is enabled.
    /// </summary>
    public bool IsSecondaryButtonEnabled
    {
        get => (bool)GetValue(IsSecondaryButtonEnabledProperty);
        set => SetValue(IsSecondaryButtonEnabledProperty, value);
    }

    /// <summary>
    /// Dependency property for CloseButtonStyle.
    /// </summary>
    public static readonly DependencyProperty CloseButtonStyleProperty = DependencyProperty.Register(
        name: nameof(CloseButtonStyle),
        propertyType: typeof(Style),
        ownerType: typeof(DialogPresenterButtons),
        typeMetadata: new(defaultValue: new Style())
    );

    /// <summary>
    /// The style to apply tot he close button.
    /// </summary>
    public Style CloseButtonStyle
    {
        get => (Style)GetValue(CloseButtonStyleProperty);
        set => SetValue(CloseButtonStyleProperty, value);
    }

    /// <summary>
    /// Dependency property for CloseButtonText.
    /// </summary>
    public static readonly DependencyProperty CloseButtonTextProperty = DependencyProperty.Register(
        name: nameof(CloseButtonText),
        propertyType: typeof(string),
        ownerType: typeof(DialogPresenterButtons),
        typeMetadata: new(
            defaultValue: null,
            propertyChangedCallback: (sender, _) => UpdateButtonLayout(sender)
        )
    );

    /// <summary>
    /// The text for the close button.
    /// If the value is null or an empty string the button will not be visible.
    /// </summary>
    public string? CloseButtonText
    {
        get => (string)GetValue(CloseButtonTextProperty);
        set => SetValue(CloseButtonTextProperty, value);
    }

    /// <summary>
    /// Dependency property for IsCloseButtonEnabled.
    /// </summary>
    public static readonly DependencyProperty IsCloseButtonEnabledProperty = DependencyProperty.Register(
        name: nameof(IsCloseButtonEnabled),
        propertyType: typeof(bool),
        ownerType: typeof(DialogPresenterButtons),
        typeMetadata: new(defaultValue: true)
    );

    /// <summary>
    /// Indicates if the close button is enabled.
    /// </summary>
    public bool IsCloseButtonEnabled
    {
        get => (bool)GetValue(IsCloseButtonEnabledProperty);
        set => SetValue(IsCloseButtonEnabledProperty, value);
    }

    /// <summary>
    /// Dependency property for ButtonSpacing.
    /// </summary>
    public static readonly DependencyProperty ButtonSpacingProperty = DependencyProperty.Register(
        name: nameof(ButtonSpacing),
        propertyType: typeof(GridLength),
        ownerType: typeof(DialogPresenterButtons),
        typeMetadata: new(
            defaultValue: new GridLength(0),
            propertyChangedCallback: (sender, _) => UpdateButtonLayout(sender)
        )
    );

    /// <summary>
    /// The spacing between buttons.
    /// </summary>
    public GridLength ButtonSpacing
    {
        get => (GridLength)GetValue(ButtonSpacingProperty);
        set => SetValue(ButtonSpacingProperty, value);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Display the frame control in the ContentPresenter defined in the
    /// template.
    /// </summary>
    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _primaryButton = GetTemplateChild("PrimaryButton") as Button;
        _secondaryButton = GetTemplateChild("SecondaryButton") as Button;
        _closeButton = GetTemplateChild("CloseButton") as Button;

        if (_primaryButton is not null)
            _primaryButton.Click += (sender, args) => PrimaryButtonClick?.Invoke(sender, args);
        if (_secondaryButton is not null)
            _secondaryButton.Click += (sender, args) => SecondaryButtonClick?.Invoke(sender, args);
        if (_closeButton is not null)
            _closeButton.Click += (sender, args) => CloseButtonClick?.Invoke(sender, args);

        var grid = GetTemplateChild("ButtonsGrid") as Grid;
        _firstColumn = grid?.ColumnDefinitions?.FirstOrDefault();
        _firstSpacer = grid?.ColumnDefinitions?.Skip(1)?.FirstOrDefault();
        _secondColumn = grid?.ColumnDefinitions?.Skip(2)?.FirstOrDefault();
        _secondSpacer = grid?.ColumnDefinitions?.Skip(3)?.FirstOrDefault();
        _thirdColumn = grid?.ColumnDefinitions?.Skip(4)?.FirstOrDefault();

        UpdateButtonLayout(this);
    }

    /// <summary>
    /// Update the layout for the buttons based on the current state.
    /// </summary>
    /// <param name="dependencyObject">
    /// The control to update the layout for.
    /// </param>
    private static void UpdateButtonLayout(DependencyObject dependencyObject)
    {
        if (dependencyObject is not DialogPresenterButtons control)
            return;

        if (
            control._primaryButton is not Button primaryButton ||
            control._secondaryButton is not Button secondaryButton ||
            control._closeButton is not Button closeButton
        )
            return;

        if (
            control._firstColumn is not ColumnDefinition firstColumn ||
            control._firstSpacer is not ColumnDefinition firstSpacer ||
            control._secondColumn is not ColumnDefinition secondColumn ||
            control._secondSpacer is not ColumnDefinition secondSpacer ||
            control._thirdColumn is not ColumnDefinition thirdColumn
        )
            return;

        var visibleButtons = new List<Button>();
        var hiddenButtons = new List<Button>();

        if (!string.IsNullOrEmpty(control.PrimaryButtonText))
            visibleButtons.Add(primaryButton);
        else
            hiddenButtons.Add(primaryButton);

        if (!string.IsNullOrEmpty(control.SecondaryButtonText))
            visibleButtons.Add(secondaryButton);
        else
            hiddenButtons.Add(secondaryButton);

        if (!string.IsNullOrEmpty(control.CloseButtonText))
            visibleButtons.Add(closeButton);
        else
            hiddenButtons.Add(closeButton);

        var buttonSpacing = control.ButtonSpacing;

        hiddenButtons.ForEach(
            button => button.Visibility = Visibility.Collapsed
        );

        switch (visibleButtons.Count)
        {
            case 3:
                {
                    firstColumn.Width = new GridLength(1, GridUnitType.Star);
                    firstSpacer.Width = buttonSpacing;
                    secondColumn.Width = new GridLength(1, GridUnitType.Star);
                    secondSpacer.Width = buttonSpacing;
                    thirdColumn.Width = new GridLength(1, GridUnitType.Star);

                    Grid.SetColumn(visibleButtons[0], 0);
                    Grid.SetColumn(visibleButtons[1], 2);
                    Grid.SetColumn(visibleButtons[2], 4);

                    visibleButtons.ForEach(
                        button => button.Visibility = Visibility.Visible
                    );

                    break;
                }

            case 2:
                {
                    firstColumn.Width = new GridLength(1, GridUnitType.Star);
                    firstSpacer.Width = buttonSpacing;
                    secondColumn.Width = new GridLength(0);
                    secondSpacer.Width = new GridLength(0);
                    thirdColumn.Width = new GridLength(1, GridUnitType.Star);

                    Grid.SetColumn(visibleButtons[0], 0);
                    Grid.SetColumn(visibleButtons[1], 4);

                    visibleButtons.ForEach(
                        button => button.Visibility = Visibility.Visible
                    );

                    break;
                }

            case 1:
                {
                    firstColumn.Width = new GridLength(1, GridUnitType.Star);
                    firstSpacer.Width = new GridLength(0);
                    secondColumn.Width = new GridLength(0);
                    secondSpacer.Width = new GridLength(0);
                    thirdColumn.Width = new GridLength(1, GridUnitType.Star);

                    Grid.SetColumn(visibleButtons[0], 4);

                    visibleButtons.ForEach(
                        button => button.Visibility = Visibility.Visible
                    );

                    break;
                }

            default:
                break;
        }
    }

    #endregion

    #region Events

    /// <summary>
    /// Raised when the primary button is clicked.
    /// </summary>
    public event RoutedEventHandler? PrimaryButtonClick;

    /// <summary>
    /// Raised when the secondary button is clicked.
    /// </summary>
    public event RoutedEventHandler? SecondaryButtonClick;

    /// <summary>
    /// Raised when the close button is clicked.
    /// </summary>
    public event RoutedEventHandler? CloseButtonClick;

    #endregion
}
