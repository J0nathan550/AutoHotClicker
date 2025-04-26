using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AutoHotClicker.Input;

public class KeybindRecorder
{
    private readonly Window? _ownerWindow;
    private TextBox? _activeTextBox;
    public bool _isRecording = false;
    private Action? _callback;

    public KeybindRecorder(Window ownerWindow)
    {
        _ownerWindow = ownerWindow;

        // Register event handlers at the window level
        _ownerWindow.PreviewKeyDown += Window_PreviewKeyDown;
        // _ownerWindow.PreviewMouseDown += Window_PreviewMouseDown;
    }

    /// <summary>
    /// Start recording keyboard/mouse input for the specified TextBox
    /// </summary>
    public void StartRecording(TextBox textBox, Action? callback = null)
    {
        _callback = callback;
        _activeTextBox = textBox;
        _isRecording = true;
        _activeTextBox.Text = "Press a key...";
        _activeTextBox.Focus();
    }

    /// <summary>
    /// Stop recording keyboard/mouse input
    /// </summary>
    public void StopRecording()
    {
        Keyboard.ClearFocus();
        _isRecording = false;
        _activeTextBox = null;
    }

    /// <summary>
    /// Handle keyboard input when recording
    /// </summary>
    private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (!_isRecording || _activeTextBox == null)
            return;

        // Don't record modifier keys alone
        if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl ||
            e.Key == Key.LeftAlt || e.Key == Key.RightAlt ||
            e.Key == Key.LeftShift || e.Key == Key.RightShift ||
            e.Key == Key.LWin || e.Key == Key.RWin)
        {
            return;
        }

        // Special case: Escape key should cancel recording
        if (e.Key == Key.Escape)
        {
            _activeTextBox.Text = string.Empty;
            StopRecording();
            e.Handled = true;
            return;
        }

        // Record the key combination
        StringBuilder hotkeyText = new();

        // Add modifier keys
        if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            hotkeyText.Append("Ctrl + ");

        if ((Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
            hotkeyText.Append("Alt + ");

        if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
            hotkeyText.Append("Shift + ");

        if ((Keyboard.Modifiers & ModifierKeys.Windows) == ModifierKeys.Windows)
            hotkeyText.Append("Win + ");

        // Add the actual key
        hotkeyText.Append(e.Key.ToString());

        // Update the textbox
        _activeTextBox.Text = hotkeyText.ToString();

        _callback?.Invoke();

        // Stop recording after a key is pressed
        StopRecording();

        // Mark the event as handled to prevent other controls from processing it
        e.Handled = true;
    }

    /*
    /// <summary>
    /// Handle mouse input when recording
    /// </summary>
    private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (!_isRecording || _activeTextBox == null)
            return;

        // Record the mouse and key combination
        StringBuilder hotkeyText = new();

        // Add modifier keys
        if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            hotkeyText.Append("Ctrl + ");

        if ((Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
            hotkeyText.Append("Alt + ");

        if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
            hotkeyText.Append("Shift + ");

        if ((Keyboard.Modifiers & ModifierKeys.Windows) == ModifierKeys.Windows)
            hotkeyText.Append("Win + ");

        // Add the mouse button
        hotkeyText.Append("Mouse" + e.ChangedButton.ToString());

        // Update the textbox
        _activeTextBox.Text = hotkeyText.ToString();

        _callback?.Invoke();

        // Stop recording after a mouse button is pressed
        StopRecording();

        // Mark the event as handled
        e.Handled = true;
    }
    */
}