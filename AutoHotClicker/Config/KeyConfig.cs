using System.Collections.ObjectModel;
using System.Media;
using System.Windows.Input;
using AutoHotClicker.Input;
using AutoHotClicker.Sounds;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using NHotkey;
using NHotkey.Wpf;
using WindowsInput;

namespace AutoHotClicker;

public partial class KeyConfig : ObservableObject
{
    [ObservableProperty]
    private KeyActions? _startStopKeybind;
    public ObservableCollection<KeyActions> KeyActions { get; set; } = [];

    public event EventHandler<bool>? OnRunningAutoKeyClickerThread;

    [JsonIgnore]
    public bool _isRunningAutoKeyClickerThread = false;
    
    private Thread? _autoKeyClickerThread;

    partial void OnStartStopKeybindChanged(KeyActions? value)
    {
        if (value != null)
        {
            if (_startStopKeybind != null && _startStopKeybind.Key != null && _startStopKeybind.Modifiers == ModifierKeys.None)
            {
                HotkeyManager.Current.AddOrReplace("StartOrStopKeyBind", _startStopKeybind.Key.Value, ModifierKeys.None, OnStartOrStopKeybind);
            }
            else if (_startStopKeybind != null && _startStopKeybind.Key != null && _startStopKeybind.Modifiers != null && _startStopKeybind.Modifiers != ModifierKeys.None)
            {
                HotkeyManager.Current.AddOrReplace("StartOrStopKeyBind", new KeyGesture(_startStopKeybind.Key.Value, _startStopKeybind.Modifiers.Value), OnStartOrStopKeybind);
            }
        }
    }

    private void OnStartOrStopKeybind(object? sender, HotkeyEventArgs e)
    {
        if (KeyActions.Count == 0 && MainWindow.KeybindRecorder != null && MainWindow.KeybindRecorder._isRecording)
        {
            return;
        }
        if (_isRunningAutoKeyClickerThread)
        {
            AudioPlayer.PlayEmbeddedMp3("AutoHotClicker.Sounds.stop.mp3");
            _isRunningAutoKeyClickerThread = false;
            OnRunningAutoKeyClickerThread?.Invoke(this, _isRunningAutoKeyClickerThread);
            return;
        }
        _autoKeyClickerThread = new(() =>
        {
            _isRunningAutoKeyClickerThread = true;
            OnRunningAutoKeyClickerThread?.Invoke(this, _isRunningAutoKeyClickerThread);
            InputSimulator inputSimulator = new();
            while (_isRunningAutoKeyClickerThread)
            {
                if (!_isRunningAutoKeyClickerThread)
                {
                    break;
                }
                foreach (KeyActions keyAction in KeyActions)
                {
                    switch (keyAction.Action)
                    {
                        case KeyAction.KeyPress:
                            if (keyAction.Key != null && keyAction.Modifiers == ModifierKeys.None)
                            {
                                inputSimulator.Keyboard.KeyDown(KeyMapper.MapToVirtualKeyCode(keyAction.Key.Value));
                            }
                            else if (keyAction.Key != null && keyAction.Modifiers != null && keyAction.Modifiers != ModifierKeys.None)
                            {
                                foreach (WindowsInput.Native.VirtualKeyCode modifierKey in KeyMapper.MapModifierKeys(keyAction.Modifiers.Value))
                                {
                                    inputSimulator.Keyboard.KeyDown(modifierKey);
                                }
                                inputSimulator.Keyboard.KeyDown(KeyMapper.MapToVirtualKeyCode(keyAction.Key.Value));
                            }
                            break;
                        case KeyAction.KeyRelease:
                            if (keyAction.Key != null && keyAction.Modifiers == ModifierKeys.None)
                            {
                                inputSimulator.Keyboard.KeyUp(KeyMapper.MapToVirtualKeyCode(keyAction.Key.Value));
                            }
                            else if (keyAction.Key != null && keyAction.Modifiers != null && keyAction.Modifiers != ModifierKeys.None)
                            {
                                inputSimulator.Keyboard.KeyUp(KeyMapper.MapToVirtualKeyCode(keyAction.Key.Value));
                                foreach (WindowsInput.Native.VirtualKeyCode modifierKey in KeyMapper.MapModifierKeys(keyAction.Modifiers.Value))
                                {
                                    inputSimulator.Keyboard.KeyUp(modifierKey);
                                }
                            }
                            break;
                    }

                    Thread.Sleep(keyAction.Delay);
                }
            }
        });
        _autoKeyClickerThread.Start();
        AudioPlayer.PlayEmbeddedMp3("AutoHotClicker.Sounds.play.mp3");
    }

}