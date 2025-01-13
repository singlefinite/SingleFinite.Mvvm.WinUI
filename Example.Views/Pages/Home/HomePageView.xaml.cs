using SingleFinite.Example.Models.Pages.Home;
using Microsoft.UI.Xaml.Controls;
using SingleFinite.Mvvm;

namespace SingleFinite.Example.Views.Pages.Home;

public sealed partial class HomePageView : UserControl, IView<HomePageViewModel>
{
    public HomePageView(HomePageViewModel viewModel)
    {
        this.InitializeComponent();
        ViewModel = viewModel;
    }

    public HomePageViewModel ViewModel { get; }
}
