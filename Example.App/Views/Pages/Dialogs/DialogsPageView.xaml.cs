using SingleFinite.Example.Models.Pages.Dialogs;
using Microsoft.UI.Xaml.Controls;
using SingleFinite.Mvvm;

namespace SingleFinite.Example.App.Views.Pages;

public sealed partial class DialogsPageView : UserControl, IView<DialogsPageViewModel>
{
    public DialogsPageView(DialogsPageViewModel viewModel)
    {
        this.InitializeComponent();
        ViewModel = viewModel;
    }

    public DialogsPageViewModel ViewModel { get; }
}
