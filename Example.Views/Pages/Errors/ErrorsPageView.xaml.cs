using SingleFinite.Example.Models.Pages.Errors;
using Microsoft.UI.Xaml.Controls;
using SingleFinite.Mvvm;

namespace SingleFinite.Example.Views.Pages.Errors;

public sealed partial class ErrorsPageView : UserControl, IView<ErrorsPageViewModel>
{
    public ErrorsPageView(ErrorsPageViewModel viewModel)
    {
        this.InitializeComponent();
        ViewModel = viewModel;
    }

    public ErrorsPageViewModel ViewModel { get; }
}
