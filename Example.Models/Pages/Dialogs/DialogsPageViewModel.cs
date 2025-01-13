using SingleFinite.Mvvm;
using SingleFinite.Mvvm.Services;

namespace SingleFinite.Example.Models.Pages.Dialogs;

public partial class DialogsPageViewModel(
    IDialogs dialogs
) : ViewModel
{
    public void ShowFirstDialog()
    {
        dialogs.Show<FirstDialogViewModel>();
    }
}
