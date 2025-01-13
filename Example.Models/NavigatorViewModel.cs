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

using SingleFinite.Example.Models.Pages.Dialogs;
using SingleFinite.Example.Models.Pages.Errors;
using SingleFinite.Example.Models.Pages.Frame;
using SingleFinite.Example.Models.Pages.Home;
using SingleFinite.Example.Models.Pages.MappedProperties;
using SingleFinite.Mvvm;
using SingleFinite.Mvvm.Services;

namespace SingleFinite.Example.Models;

public partial class NavigatorViewModel(
    IPresentableItem presentableItem
) : ViewModel
{
    public string? SelectedPage
    {
        get => field;
        set => ChangeProperty(
            field: ref field,
            value: value,
            onPropertyChanged: () => UpdatePage(value)
        );
    }

    public IPresentableItem PresentableItem => presentableItem;

    protected override void OnInitialize()
    {
        SelectedPage = NavigatorPages.Home;
    }

    private void UpdatePage(string? page)
    {
        var viewModelType = page switch
        {
            NavigatorPages.Home => typeof(HomePageViewModel),
            NavigatorPages.Dialogs => typeof(DialogsPageViewModel),
            NavigatorPages.Frame => typeof(FramePageViewModel),
            NavigatorPages.MappedProperties => typeof(MappedPropertiesPageViewModel),
            NavigatorPages.Errors => typeof(ErrorsPageViewModel),
            _ => null
        };

        if (presentableItem.Current?.ViewModel?.GetType() == viewModelType)
            return;

        if (viewModelType is null)
        {
            presentableItem.Clear();
            return;
        }

        presentableItem.Set(new ViewModelDescriptor(viewModelType));
    }
}
