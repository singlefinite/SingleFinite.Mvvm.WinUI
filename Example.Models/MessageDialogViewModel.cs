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

using SingleFinite.Mvvm;

namespace SingleFinite.Example.Models;

public partial class MessageDialogViewModel(
    MessageDialogViewModel.Context? context = null
) :
    ViewModel,
    IClosable
{
    public MessageResult Result { get; private set; } = MessageResult.Cancel;

    public string Title
    {
        get => field;
        set => ChangeProperty(ref field, value);
    } = context?.Title ?? string.Empty;

    public string Message
    {
        get => field;
        set => ChangeProperty(ref field, value);
    } = context?.Message ?? string.Empty;

    public string PrimaryText
    {
        get => field;
        set => ChangeProperty(ref field, value);
    } = context?.PrimaryText ?? string.Empty;

    public string SecondaryText
    {
        get => field;
        set => ChangeProperty(ref field, value);
    } = context?.SecondaryText ?? string.Empty;

    public string CancelText
    {
        get => field;
        set => ChangeProperty(ref field, value);
    } = context?.CancelText ?? string.Empty;

    public void PrimaryClick()
    {
        Result = MessageResult.Primary;
        Close();
    }

    public void SecondaryClick()
    {
        Result = MessageResult.Secondary;
        Close();
    }

    public void CancelClick()
    {
        Result = MessageResult.Cancel;
        Close();
    }

    private void Close() => _closedSource.RaiseEvent(this);

    public Observable<IClosable> Closed => _closedSource.Observable;
    private readonly ObservableSource<IClosable> _closedSource = new();

    public record class Context(
        string Title = "",
        string Message = "",
        string PrimaryText = "",
        string SecondaryText = "",
        string CancelText = ""
    );

    public enum MessageResult
    {
        Primary,
        Secondary,
        Cancel
    }
}
