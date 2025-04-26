using System.Text;
using System.Windows.Input;

namespace AutoHotClicker.Input;

public class KeyActions
{
    public KeyAction Action { get; set; }
    public ModifierKeys? Modifiers { get; set; }
    public Key? Key { get; set; }
    public string KeyString { get => ToString(); }
    public int Delay { get; set; }

    public override string ToString()
    {
        StringBuilder sb = new();
        if ((Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            sb.Append("Ctrl + ");
        if ((Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
            sb.Append("Alt + ");
        if ((Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
            sb.Append("Shift + ");
        if ((Modifiers & ModifierKeys.Windows) == ModifierKeys.Windows)
            sb.Append("Win + ");

        sb.Append(Key);

        return sb.ToString();
    }
}

public enum KeyAction
{
    KeyPress,
    KeyRelease,
    Timer // will just use delay with no key action
}