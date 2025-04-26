using System.Windows.Controls;
using System.Windows.Input;

namespace AutoHotClicker.Input;

public static class KeyExtensions
{
    /// <summary>
    /// Attach a key recorder to a TextBox
    /// </summary>
    public static void AttachKeyRecorder(this TextBox textBox, KeybindRecorder recorder, Action? callback = null)
    {
        textBox.GotFocus += (sender, e) =>
        {
            recorder.StartRecording(textBox, callback);
        };
        textBox.GotMouseCapture += (sender, e) =>
        {
            recorder.StartRecording(textBox, callback);
        };
    }

    /// <summary>
    /// Updates the passed class variables from the Keybind textbox
    /// </summary>
    public static KeyActions CreateOrUpdateHotkeyData(TextBox control)
    {
        string hotkeyText = control.Text;
        // Parse the hotkey text to create a KeyActions object
        ModifierKeys modifiers = ModifierKeys.None;
        Key key = Key.None;
        if (hotkeyText.Contains("Ctrl"))
            modifiers |= ModifierKeys.Control;
        if (hotkeyText.Contains("Alt"))
            modifiers |= ModifierKeys.Alt;
        if (hotkeyText.Contains("Shift"))
            modifiers |= ModifierKeys.Shift;
        if (hotkeyText.Contains("Win"))
            modifiers |= ModifierKeys.Windows;
        else
        {
            // Extract the key part
            string keyText = hotkeyText;
            foreach (string? modifier in new[] { "Ctrl + ", "Alt + ", "Shift + ", "Win + " })
            {
                keyText = keyText.Replace(modifier, "");
            }
            // Parse the key
            if (Enum.TryParse(keyText, out Key parsedKey))
                key = parsedKey;
        }
        return new KeyActions
        {
            Modifiers = modifiers,
            Key = key
        };
    }
}