using SingleFinite.Example.Models.Pages.MappedProperties;
using Microsoft.UI.Xaml.Controls;
using SingleFinite.Mvvm;

namespace SingleFinite.Example.Views.Pages.MappedProperties;

public sealed partial class MappedPropertiesPageView : UserControl, IView<MappedPropertiesPageViewModel>
{
    public MappedPropertiesPageView(MappedPropertiesPageViewModel viewModel)
    {
        this.InitializeComponent();
        ViewModel = viewModel;
    }

    public MappedPropertiesPageViewModel ViewModel { get; }
}
