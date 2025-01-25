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

using System;
using System.Runtime.InteropServices;

namespace SingleFinite.Mvvm.WinUI.Internal;

/// <summary>
/// Imported Win32 functions.
/// </summary>
internal static partial class Win32
{
    /// <summary>
    /// Brings the thread that created the specified window into the foreground 
    /// and activates the window. Keyboard input is directed to the window, and 
    /// various visual cues are changed for the user. The system assigns a 
    /// slightly higher priority to the thread that created the foreground 
    /// window than it does to other threads.
    /// </summary>
    /// <param name="hWnd">A handle to the window that should be activated and 
    /// brought to the foreground.</param>
    /// <returns>
    /// If the window was brought to the foreground, the return value is true.
    /// If the window was not brought to the foreground, the return value is 
    /// false.
    /// </returns>
    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool SetForegroundWindow(IntPtr hWnd);
}
