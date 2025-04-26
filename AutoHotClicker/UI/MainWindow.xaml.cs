using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AutoHotClicker.Config;
using AutoHotClicker.Input;
using Microsoft.Win32;
using ModernWpf.Controls;
using ModernWpf.Controls.Primitives;
using NHotkey.Wpf;

namespace AutoHotClicker;

public partial class MainWindow : Window
{
    private static KeybindRecorder? keybindRecorder;
    private readonly ProgramConfig _programConfig;
    private KeyConfig _keyConfig;

    public static KeybindRecorder? KeybindRecorder { get => keybindRecorder; set => keybindRecorder = value; }

    public MainWindow()
    {
        InitializeComponent();

        ConfigManager<ProgramConfig> configManager = new("Data/config.json");
        configManager.Load();
        _programConfig = configManager.Config;

        ConfigManager<KeyConfig> keyManager = new(_programConfig.LastConfigPath);
        keyManager.Load();
        _keyConfig = keyManager.Config;

        KeybindRecorder = new KeybindRecorder(this);
        StartStopActionsHotKey.AttachKeyRecorder(KeybindRecorder, new Action(() =>
        {
            KeyActions keyActions = new();
            keyActions = KeyExtensions.CreateOrUpdateHotkeyData(StartStopActionsHotKey);
            _keyConfig.StartStopKeybind = keyActions;
        }));

        if (_keyConfig.StartStopKeybind != null) StartStopActionsHotKey.Text = _keyConfig.StartStopKeybind.ToString();
        _keyConfig.OnRunningAutoKeyClickerThread += KeyConfig_OnRunningAutoKeyClickerThread;
        KeyListView.ItemsSource = _keyConfig.KeyActions;
    }

    private void KeyConfig_OnRunningAutoKeyClickerThread(object? sender, bool isThreadRunning)
    {
        if (isThreadRunning)
        {
            Dispatcher.Invoke(() =>
            {
                StatusTextBox.Text = "Working!";
            });
        }
        else
        {
            Dispatcher.Invoke(() =>
            {
                StatusTextBox.Text = "Not Working!";
            });
        }
    }

    private async void CreateNewKeyConfigButton_Click(object sender, RoutedEventArgs e)
    {
        if (await VerifyUserWishToStartNewKeyConfig())
        {
            ClearKeyConfig();
        }
    }

    private async Task<bool> VerifyUserWishToStartNewKeyConfig()
    {
        if (_keyConfig.KeyActions.Count > 0)
        {
            ContentDialog contentDialog = new()
            {
                Title = "Resetting config key file",
                Content = "You have elements inside of list that are probably unsaved.\nWould you like to anyway start new key config file?",
                CloseButtonText = "Cancel",
                PrimaryButtonText = "Reset"
            };
            ContentDialogResult response = await contentDialog.ShowAsync();
            if (response == ContentDialogResult.Primary)
            {
                return true;
            }
            return false;
        }
        else
        {
            return true;
        }
    }

    private void ClearKeyConfig()
    {
        _keyConfig.OnRunningAutoKeyClickerThread -= KeyConfig_OnRunningAutoKeyClickerThread;
        HotkeyManager.Current.AddOrReplace("StartOrStopKeyBind", new KeyGesture(Key.None, ModifierKeys.None), null);
        _keyConfig = new();
        StartStopActionsHotKey.Text = string.Empty;
        StatusTextBox.Text = "Not Working!";
        KeyListView.ItemsSource = null;
        //KeyListView.Items.Clear();
    }

    private async void OpenKeyConfigButton_Click(object sender, RoutedEventArgs e)
    {
        if (await VerifyUserWishToStartNewKeyConfig())
        {
            OpenFileDialog openFileDialog = new()
            {
                Title = "AutoHotClicker | Select key config file...",
                Filter = "JSON files (*.json)|*.json"
            };
            openFileDialog.ShowDialog();

            if (openFileDialog.FileName != string.Empty)
            {
                _keyConfig.OnRunningAutoKeyClickerThread -= KeyConfig_OnRunningAutoKeyClickerThread;
                HotkeyManager.Current.AddOrReplace("StartOrStopKeyBind", new KeyGesture(Key.None, ModifierKeys.None), null);

                _programConfig.LastConfigPath = openFileDialog.FileName;
                ConfigManager<ProgramConfig> configManager = new("Data/config.json");
                configManager.Config.LastConfigPath = _programConfig.LastConfigPath;
                configManager.Save();

                ConfigManager<KeyConfig> keyManager = new(_programConfig.LastConfigPath);
                keyManager.Load();
                _keyConfig = keyManager.Config;
                _keyConfig.OnRunningAutoKeyClickerThread += KeyConfig_OnRunningAutoKeyClickerThread;

                if (_keyConfig.StartStopKeybind != null) StartStopActionsHotKey.Text = _keyConfig.StartStopKeybind.ToString();
                StatusTextBox.Text = "Not Working!";
                KeyListView.ItemsSource = _keyConfig.KeyActions;
            }
        }
    }

    private void SaveKeyConfigButton_Click(object sender, RoutedEventArgs e)
    {
        SaveFileDialog saveFileDialog = new()
        {
            Title = "AutoHotClicker | Select key path to save config file...",
            Filter = "JSON files (*.json)|*.json"
        };
        saveFileDialog.ShowDialog();

        if (saveFileDialog.FileName != string.Empty)
        {
            _programConfig.LastConfigPath = saveFileDialog.FileName;
            ConfigManager<ProgramConfig> configManager = new("Data/config.json");
            configManager.Config.LastConfigPath = _programConfig.LastConfigPath;
            configManager.Save();

            ConfigManager<KeyConfig> keyManager = new(_programConfig.LastConfigPath);
            keyManager.Config.StartStopKeybind = _keyConfig.StartStopKeybind;
            keyManager.Config.KeyActions = _keyConfig.KeyActions;
            keyManager.Save();
        }
    }

    private void CreateKeyActionButton_Click(object sender, RoutedEventArgs e)
    {
        if (KeybindRecorder == null)
        {
            return;
        }

        KeyAction keyAction = KeyAction.Timer;

        TextBox keybindTextbox = new();
        keybindTextbox.AttachKeyRecorder(KeybindRecorder);
        ControlHelper.SetHeader(keybindTextbox, "Keybind:");
        ControlHelper.SetCornerRadius(keybindTextbox, new CornerRadius(0));

        TextBox delayTextbox = new();
        ControlHelper.SetHeader(delayTextbox, "Delay:");
        ControlHelper.SetPlaceholderText(delayTextbox, "Set time in milliseconds (1 second = 1000)");
        ControlHelper.SetCornerRadius(delayTextbox, new CornerRadius(0));

        ComboBox keyActionComboBox = new()
        {
            ItemsSource = Enum.GetValues(typeof(KeyAction)),
            SelectedItem = keyAction,
            SelectedIndex = 0
        };
        ControlHelper.SetHeader(keyActionComboBox, "Key action:");

        ContentDialog createKeyDialog = new()
        {
            Title = "Create new key action",
            Content = new SimpleStackPanel()
            {
                Spacing = 5,
                Children =
                {
                    keybindTextbox,
                    delayTextbox,
                    keyActionComboBox
                }
            },
            PrimaryButtonText = "Create",
            CloseButtonText = "Cancel"
        };
        createKeyDialog.PrimaryButtonClick += (s, args) =>
        {
            KeyActions keyActions = new();
            keyActions = KeyExtensions.CreateOrUpdateHotkeyData(keybindTextbox);

            if (keyActionComboBox.SelectedItem != null)
            {
                keyAction = (KeyAction)keyActionComboBox.SelectedItem;
            }
            else
            {
                MessageBox.Show("Please select action in dropdown.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                args.Cancel = true;
                return;
            }

            if (string.IsNullOrEmpty(keybindTextbox.Text) && keyAction != KeyAction.Timer)
            {
                MessageBox.Show("Please select keybind in keybind box.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                args.Cancel = true;
                return;
            }

            if (int.TryParse(delayTextbox.Text, out int delay))
            {
                keyActions.Action = keyAction;
                keyActions.Delay = delay;
                _keyConfig.KeyActions.Add(keyActions);
            }
            else
            {
                MessageBox.Show("Please enter a valid number for delay.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                args.Cancel = true;
                return;
            }
            KeyListView.ItemsSource = null;
            KeyListView.ItemsSource = _keyConfig.KeyActions;
        };
        createKeyDialog.ShowAsync();
    }

    private void EditKeyActionButton_Click(object sender, RoutedEventArgs e)
    {
        if (KeyListView.SelectedItem != null && KeybindRecorder != null)
        {
            if (_keyConfig._isRunningAutoKeyClickerThread)
            {
                _keyConfig._isRunningAutoKeyClickerThread = false;
            }

            // Get the selected item and its index
            int selectedIndex = KeyListView.SelectedIndex;
            KeyActions keyActions = (KeyActions)KeyListView.SelectedItem;
            KeyAction keyAction = keyActions.Action; // Use the current action

            TextBox keybindTextbox = new()
            {
                Text = keyActions.ToString()
            };
            keybindTextbox.AttachKeyRecorder(KeybindRecorder);
            ControlHelper.SetHeader(keybindTextbox, "Keybind:");
            ControlHelper.SetCornerRadius(keybindTextbox, new CornerRadius(0));

            TextBox delayTextbox = new()
            {
                Text = keyActions.Delay.ToString()
            };
            ControlHelper.SetHeader(delayTextbox, "Delay:");
            ControlHelper.SetPlaceholderText(delayTextbox, "Set time in milliseconds (1 second = 1000)");
            ControlHelper.SetCornerRadius(delayTextbox, new CornerRadius(0));

            ComboBox keyActionComboBox = new()
            {
                ItemsSource = Enum.GetValues(typeof(KeyAction)),
                SelectedItem = keyAction
            };
            ControlHelper.SetHeader(keyActionComboBox, "Key action:");

            ContentDialog editKeyDialog = new()
            {
                Title = "Edit key action",
                Content = new SimpleStackPanel()
                {
                    Spacing = 5,
                    Children =
                    {
                        keybindTextbox,
                        delayTextbox,
                        keyActionComboBox
                    }
                },
                PrimaryButtonText = "Edit",
                CloseButtonText = "Cancel"
            };

            editKeyDialog.PrimaryButtonClick += (s, args) =>
            {
                KeyActions updatedKeyAction = KeyExtensions.CreateOrUpdateHotkeyData(keybindTextbox);

                if (keyActionComboBox.SelectedItem != null)
                {
                    updatedKeyAction.Action = (KeyAction)keyActionComboBox.SelectedItem;
                }
                else
                {
                    MessageBox.Show("Please select action in dropdown.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    args.Cancel = true;
                    return;
                }

                if (string.IsNullOrEmpty(keybindTextbox.Text) && updatedKeyAction.Action != KeyAction.Timer)
                {
                    MessageBox.Show("Please select keybind in keybind box.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    args.Cancel = true;
                    return;
                }

                if (int.TryParse(delayTextbox.Text, out int delay))
                {
                    updatedKeyAction.Delay = delay;

                    // Remove the old item and insert the updated one at the same position
                    _keyConfig.KeyActions.RemoveAt(selectedIndex);
                    _keyConfig.KeyActions.Insert(selectedIndex, updatedKeyAction);
                }
                else
                {
                    MessageBox.Show("Please enter a valid number for delay.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    args.Cancel = true;
                    return;
                }

                KeyListView.ItemsSource = null;
                KeyListView.ItemsSource = _keyConfig.KeyActions;
                KeyListView.SelectedIndex = selectedIndex; // Maintain selection
            };

            editKeyDialog.ShowAsync();
        }
    }

    private async void RemoveKeyActionButton_Click(object sender, RoutedEventArgs e)
    {
        if (KeyListView.SelectedItem != null)
        {
            ContentDialog removeDialog = new()
            {
                Title = "Remove key action",
                Content = "Are you sure you want to remove this key action?",
                CloseButtonText = "Cancel",
                PrimaryButtonText = "Remove"
            };
            ContentDialogResult response = await removeDialog.ShowAsync();
            if (response == ContentDialogResult.Primary)
            {
                KeyActions keyActions = (KeyActions)KeyListView.SelectedItem;
                _keyConfig.KeyActions.Remove(keyActions);
                KeyListView.ItemsSource = null;
                KeyListView.ItemsSource = _keyConfig.KeyActions;
            }
        }
    }

    private void GitHubButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            const string githubUrl = "https://github.com/J0nathan550/AutoHotClicker";
            Process.Start(new ProcessStartInfo
            {
                FileName = githubUrl,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to open GitHub page: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}