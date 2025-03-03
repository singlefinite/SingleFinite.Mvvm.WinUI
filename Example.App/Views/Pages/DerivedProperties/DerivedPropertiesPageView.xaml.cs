using SingleFinite.Example.Models.Pages.DerivedProperties;
using Microsoft.UI.Xaml.Controls;
using SingleFinite.Mvvm;

namespace SingleFinite.Example.App.Views.Pages.DerivedProperties;

public sealed partial class DerivedPropertiesPageView : UserControl, IView<DerivedPropertiesPageViewModel>
{
    public DerivedPropertiesPageView(DerivedPropertiesPageViewModel viewModel)
    {
        this.InitializeComponent();
        ViewModel = viewModel;
    }

    public DerivedPropertiesPageViewModel ViewModel { get; }
}
