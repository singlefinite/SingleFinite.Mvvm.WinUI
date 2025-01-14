using SingleFinite.Example.Models.Pages.Frame;
using Microsoft.UI.Xaml.Controls;
using SingleFinite.Mvvm;

namespace SingleFinite.Example.App.Views.Pages.Frame;

public sealed partial class FramePageView : UserControl, IView<FramePageViewModel>
{
    public FramePageView(FramePageViewModel viewModel)
    {
        this.InitializeComponent();
        ViewModel = viewModel;
    }

    public FramePageViewModel ViewModel { get; }
}
