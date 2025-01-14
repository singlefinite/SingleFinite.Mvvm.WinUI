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

using System.Linq;
using SingleFinite.Example.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SingleFinite.Mvvm;

namespace SingleFinite.Example.App.Views;

public sealed partial class NavigatorView : UserControl, IView<NavigatorViewModel>
{
    public NavigatorView(NavigatorViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;

        ViewModel.MapProperty(
            mappedObject: this,
            mappedPropertyName: nameof(SelectedMenuItem),
            sourcePropertyNames: nameof(ViewModel.SelectedPage)
        );
    }

    public NavigatorViewModel ViewModel { get; }

    public object? SelectedMenuItem
    {
        get => navigationView.MenuItems.FirstOrDefault(menuItem =>
            ((menuItem as FrameworkElement)?.Tag as string) == ViewModel.SelectedPage
        );
        set => ViewModel.SelectedPage =
            (navigationView.SelectedItem as FrameworkElement)?.Tag as string;
    }
}
