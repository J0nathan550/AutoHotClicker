using System.Windows.Input;
using WindowsInput.Native;

namespace AutoHotClicker.Input;

public static class KeyMapper
{
    private static readonly Dictionary<Key, VirtualKeyCode> KeyToVirtualKeyCodeMap = new()
    {
        // Special Keys
        { Key.None, VirtualKeyCode.NONAME },
        { Key.Cancel, VirtualKeyCode.CANCEL },
        { Key.Back, VirtualKeyCode.BACK },
        { Key.Tab, VirtualKeyCode.TAB },
        { Key.Clear, VirtualKeyCode.CLEAR },
        { Key.Enter, VirtualKeyCode.RETURN },
        // { Key.Return, VirtualKeyCode.RETURN }, duplicate
        { Key.Pause, VirtualKeyCode.PAUSE },
        // { Key.Capital, VirtualKeyCode.CAPITAL }, duplicate
        { Key.CapsLock, VirtualKeyCode.CAPITAL },
        { Key.HangulMode, VirtualKeyCode.HANGUL },
        // { Key.KanaMode, VirtualKeyCode.KANA }, duplicate somehow??? probably unused
        { Key.JunjaMode, VirtualKeyCode.JUNJA },
        { Key.FinalMode, VirtualKeyCode.FINAL },
        // { Key.HanjaMode, VirtualKeyCode.HANJA }, duplicate somehow??? probably unused
        { Key.KanjiMode, VirtualKeyCode.KANJI },
        { Key.Escape, VirtualKeyCode.ESCAPE },
        { Key.ImeConvert, VirtualKeyCode.CONVERT },
        { Key.ImeNonConvert, VirtualKeyCode.NONCONVERT },
        { Key.ImeAccept, VirtualKeyCode.ACCEPT },
        { Key.ImeModeChange, VirtualKeyCode.MODECHANGE },
        { Key.Space, VirtualKeyCode.SPACE },

        // Navigation Keys
        { Key.PageUp, VirtualKeyCode.PRIOR },
        //{ Key.Prior, VirtualKeyCode.PRIOR }, duplicate
        // { Key.Next, VirtualKeyCode.NEXT }, duplicate
        { Key.PageDown, VirtualKeyCode.NEXT },
        { Key.End, VirtualKeyCode.END },
        { Key.Home, VirtualKeyCode.HOME },
        { Key.Left, VirtualKeyCode.LEFT },
        { Key.Up, VirtualKeyCode.UP },
        { Key.Right, VirtualKeyCode.RIGHT },
        { Key.Down, VirtualKeyCode.DOWN },
        { Key.Select, VirtualKeyCode.SELECT },
        { Key.Print, VirtualKeyCode.PRINT },
        { Key.Execute, VirtualKeyCode.EXECUTE },
        { Key.PrintScreen, VirtualKeyCode.SNAPSHOT },
        // { Key.Snapshot, VirtualKeyCode.SNAPSHOT }, duplicate
        { Key.Insert, VirtualKeyCode.INSERT },
        { Key.Delete, VirtualKeyCode.DELETE },
        { Key.Help, VirtualKeyCode.HELP },

        // Digit Keys
        { Key.D0, VirtualKeyCode.VK_0 },
        { Key.D1, VirtualKeyCode.VK_1 },
        { Key.D2, VirtualKeyCode.VK_2 },
        { Key.D3, VirtualKeyCode.VK_3 },
        { Key.D4, VirtualKeyCode.VK_4 },
        { Key.D5, VirtualKeyCode.VK_5 },
        { Key.D6, VirtualKeyCode.VK_6 },
        { Key.D7, VirtualKeyCode.VK_7 },
        { Key.D8, VirtualKeyCode.VK_8 },
        { Key.D9, VirtualKeyCode.VK_9 },

        // Letter Keys
        { Key.A, VirtualKeyCode.VK_A },
        { Key.B, VirtualKeyCode.VK_B },
        { Key.C, VirtualKeyCode.VK_C },
        { Key.D, VirtualKeyCode.VK_D },
        { Key.E, VirtualKeyCode.VK_E },
        { Key.F, VirtualKeyCode.VK_F },
        { Key.G, VirtualKeyCode.VK_G },
        { Key.H, VirtualKeyCode.VK_H },
        { Key.I, VirtualKeyCode.VK_I },
        { Key.J, VirtualKeyCode.VK_J },
        { Key.K, VirtualKeyCode.VK_K },
        { Key.L, VirtualKeyCode.VK_L },
        { Key.M, VirtualKeyCode.VK_M },
        { Key.N, VirtualKeyCode.VK_N },
        { Key.O, VirtualKeyCode.VK_O },
        { Key.P, VirtualKeyCode.VK_P },
        { Key.Q, VirtualKeyCode.VK_Q },
        { Key.R, VirtualKeyCode.VK_R },
        { Key.S, VirtualKeyCode.VK_S },
        { Key.T, VirtualKeyCode.VK_T },
        { Key.U, VirtualKeyCode.VK_U },
        { Key.V, VirtualKeyCode.VK_V },
        { Key.W, VirtualKeyCode.VK_W },
        { Key.X, VirtualKeyCode.VK_X },
        { Key.Y, VirtualKeyCode.VK_Y },
        { Key.Z, VirtualKeyCode.VK_Z },

        // Windows and App Keys
        { Key.LWin, VirtualKeyCode.LWIN },
        { Key.RWin, VirtualKeyCode.RWIN },
        { Key.Apps, VirtualKeyCode.APPS },
        { Key.Sleep, VirtualKeyCode.SLEEP },

        // Numpad Keys
        { Key.NumPad0, VirtualKeyCode.NUMPAD0 },
        { Key.NumPad1, VirtualKeyCode.NUMPAD1 },
        { Key.NumPad2, VirtualKeyCode.NUMPAD2 },
        { Key.NumPad3, VirtualKeyCode.NUMPAD3 },
        { Key.NumPad4, VirtualKeyCode.NUMPAD4 },
        { Key.NumPad5, VirtualKeyCode.NUMPAD5 },
        { Key.NumPad6, VirtualKeyCode.NUMPAD6 },
        { Key.NumPad7, VirtualKeyCode.NUMPAD7 },
        { Key.NumPad8, VirtualKeyCode.NUMPAD8 },
        { Key.NumPad9, VirtualKeyCode.NUMPAD9 },
        { Key.Multiply, VirtualKeyCode.MULTIPLY },
        { Key.Add, VirtualKeyCode.ADD },
        { Key.Separator, VirtualKeyCode.SEPARATOR },
        { Key.Subtract, VirtualKeyCode.SUBTRACT },
        { Key.Decimal, VirtualKeyCode.DECIMAL },
        { Key.Divide, VirtualKeyCode.DIVIDE },

        // Function Keys
        { Key.F1, VirtualKeyCode.F1 },
        { Key.F2, VirtualKeyCode.F2 },
        { Key.F3, VirtualKeyCode.F3 },
        { Key.F4, VirtualKeyCode.F4 },
        { Key.F5, VirtualKeyCode.F5 },
        { Key.F6, VirtualKeyCode.F6 },
        { Key.F7, VirtualKeyCode.F7 },
        { Key.F8, VirtualKeyCode.F8 },
        { Key.F9, VirtualKeyCode.F9 },
        { Key.F10, VirtualKeyCode.F10 },
        { Key.F11, VirtualKeyCode.F11 },
        { Key.F12, VirtualKeyCode.F12 },
        { Key.F13, VirtualKeyCode.F13 },
        { Key.F14, VirtualKeyCode.F14 },
        { Key.F15, VirtualKeyCode.F15 },
        { Key.F16, VirtualKeyCode.F16 },
        { Key.F17, VirtualKeyCode.F17 },
        { Key.F18, VirtualKeyCode.F18 },
        { Key.F19, VirtualKeyCode.F19 },
        { Key.F20, VirtualKeyCode.F20 },
        { Key.F21, VirtualKeyCode.F21 },
        { Key.F22, VirtualKeyCode.F22 },
        { Key.F23, VirtualKeyCode.F23 },
        { Key.F24, VirtualKeyCode.F24 },

        // Lock Keys
        { Key.NumLock, VirtualKeyCode.NUMLOCK },
        { Key.Scroll, VirtualKeyCode.SCROLL },

        // Modifier Keys
        { Key.LeftShift, VirtualKeyCode.LSHIFT },
        { Key.RightShift, VirtualKeyCode.RSHIFT },
        { Key.LeftCtrl, VirtualKeyCode.LCONTROL },
        { Key.RightCtrl, VirtualKeyCode.RCONTROL },
        { Key.LeftAlt, VirtualKeyCode.LMENU },
        { Key.RightAlt, VirtualKeyCode.RMENU },

        // Browser Keys
        { Key.BrowserBack, VirtualKeyCode.BROWSER_BACK },
        { Key.BrowserForward, VirtualKeyCode.BROWSER_FORWARD },
        { Key.BrowserRefresh, VirtualKeyCode.BROWSER_REFRESH },
        { Key.BrowserStop, VirtualKeyCode.BROWSER_STOP },
        { Key.BrowserSearch, VirtualKeyCode.BROWSER_SEARCH },
        { Key.BrowserFavorites, VirtualKeyCode.BROWSER_FAVORITES },
        { Key.BrowserHome, VirtualKeyCode.BROWSER_HOME },

        // Media Keys
        { Key.VolumeMute, VirtualKeyCode.VOLUME_MUTE },
        { Key.VolumeDown, VirtualKeyCode.VOLUME_DOWN },
        { Key.VolumeUp, VirtualKeyCode.VOLUME_UP },
        { Key.MediaNextTrack, VirtualKeyCode.MEDIA_NEXT_TRACK },
        { Key.MediaPreviousTrack, VirtualKeyCode.MEDIA_PREV_TRACK },
        { Key.MediaStop, VirtualKeyCode.MEDIA_STOP },
        { Key.MediaPlayPause, VirtualKeyCode.MEDIA_PLAY_PAUSE },
        { Key.LaunchMail, VirtualKeyCode.LAUNCH_MAIL },
        { Key.SelectMedia, VirtualKeyCode.LAUNCH_MEDIA_SELECT },
        { Key.LaunchApplication1, VirtualKeyCode.LAUNCH_APP1 },
        { Key.LaunchApplication2, VirtualKeyCode.LAUNCH_APP2 },

        // OEM Keys
        //{ Key.Oem1, VirtualKeyCode.OEM_1 }, duplicate
        { Key.OemSemicolon, VirtualKeyCode.OEM_1 },
        { Key.OemPlus, VirtualKeyCode.OEM_PLUS },
        { Key.OemComma, VirtualKeyCode.OEM_COMMA },
        { Key.OemMinus, VirtualKeyCode.OEM_MINUS },
        { Key.OemPeriod, VirtualKeyCode.OEM_PERIOD },
        //{ Key.Oem2, VirtualKeyCode.OEM_2 }, duplicate
        { Key.OemQuestion, VirtualKeyCode.OEM_2 },
        //{ Key.Oem3, VirtualKeyCode.OEM_3 }, duplicate
        { Key.OemTilde, VirtualKeyCode.OEM_3 },
        //{ Key.Oem4, VirtualKeyCode.OEM_4 }, duplicate
        { Key.OemOpenBrackets, VirtualKeyCode.OEM_4 },
        //{ Key.Oem5, VirtualKeyCode.OEM_5 }, duplicate
        { Key.OemPipe, VirtualKeyCode.OEM_5 },
        //{ Key.Oem6, VirtualKeyCode.OEM_6 }, duplicate
        { Key.OemCloseBrackets, VirtualKeyCode.OEM_6 },
        //{ Key.Oem7, VirtualKeyCode.OEM_7 }, duplicate
        { Key.OemQuotes, VirtualKeyCode.OEM_7 },
        //{ Key.Oem8, VirtualKeyCode.OEM_8 }, duplicate
        //{ Key.Oem102, VirtualKeyCode.OEM_102 }, duplicate
        { Key.OemBackslash, VirtualKeyCode.OEM_102 },
        { Key.OemClear, VirtualKeyCode.OEM_CLEAR },

        // Special Processing Keys
        { Key.ImeProcessed, VirtualKeyCode.PROCESSKEY },
        
        // Miscellaneous
        { Key.Attn, VirtualKeyCode.ATTN },
        { Key.CrSel, VirtualKeyCode.CRSEL },
        { Key.ExSel, VirtualKeyCode.EXSEL },
        { Key.EraseEof, VirtualKeyCode.EREOF },
        { Key.Play, VirtualKeyCode.PLAY },
        { Key.Zoom, VirtualKeyCode.ZOOM },
        { Key.NoName, VirtualKeyCode.NONAME },
        { Key.Pa1, VirtualKeyCode.PA1 }
    };

    private static readonly Dictionary<Key, Key> KeyAliasMap = new()
    {
        { Key.Return, Key.Enter },
        { Key.Capital, Key.CapsLock },
        { Key.Prior, Key.PageUp },
        { Key.Next, Key.PageDown },
        { Key.Snapshot, Key.PrintScreen },
        { Key.Oem1, Key.OemSemicolon },
        { Key.Oem2, Key.OemQuestion },
        { Key.Oem3, Key.OemTilde },
        { Key.Oem4, Key.OemOpenBrackets },
        { Key.Oem5, Key.OemPipe },
        { Key.Oem6, Key.OemCloseBrackets },
        { Key.Oem7, Key.OemQuotes },
        { Key.Oem8, Key.OemBackslash },
        { Key.Oem102, Key.OemBackslash }
    };

    /// <summary>
    /// Maps a WPF Key to its corresponding VirtualKeyCode
    /// </summary>
    /// <param name="key">The WPF Key to map</param>
    /// <returns>The corresponding VirtualKeyCode</returns>
    public static VirtualKeyCode MapToVirtualKeyCode(Key key)
    {
        // Handle aliases
        if (KeyAliasMap.TryGetValue(key, out Key mappedKey))
        {
            key = mappedKey;
        }

        if (KeyToVirtualKeyCodeMap.TryGetValue(key, out VirtualKeyCode virtualKeyCode))
        {
            return virtualKeyCode;
        }

        // As a fallback, try direct casting
        try
        {
            return (VirtualKeyCode)key;
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Could not map WPF Key '{key}' to VirtualKeyCode", ex);
        }
    }

    /// <summary>
    /// Maps WPF ModifierKeys to their corresponding VirtualKeyCodes
    /// </summary>
    /// <param name="modifierKeys">The WPF ModifierKeys to map</param>
    /// <returns>A list of VirtualKeyCodes corresponding to the pressed modifier keys</returns>
    public static List<VirtualKeyCode> MapModifierKeys(ModifierKeys modifierKeys)
    {
        List<VirtualKeyCode> result = new List<VirtualKeyCode>();

        if ((modifierKeys & ModifierKeys.Control) == ModifierKeys.Control)
            result.Add(VirtualKeyCode.CONTROL);

        if ((modifierKeys & ModifierKeys.Shift) == ModifierKeys.Shift)
            result.Add(VirtualKeyCode.SHIFT);

        if ((modifierKeys & ModifierKeys.Alt) == ModifierKeys.Alt)
            result.Add(VirtualKeyCode.MENU);

        if ((modifierKeys & ModifierKeys.Windows) == ModifierKeys.Windows)
            result.Add(VirtualKeyCode.LWIN);

        return result;
    }
}