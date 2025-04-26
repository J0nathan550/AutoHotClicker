using System.IO;
using Newtonsoft.Json;

namespace AutoHotClicker.Config;

/// <summary>
/// A generic configuration wrapper that handles saving and loading of configuration settings using Newtonsoft.Json
/// with stream-based operations for better memory efficiency
/// </summary>
/// <typeparam name="T">The type of the configuration object</typeparam>
/// <remarks>
/// Creates a new instance of ConfigManager with the specified file path
/// </remarks>
/// <param name="filePath">The path where the configuration file will be stored</param>
public class ConfigManager<T>(string filePath) where T : class, new()
{
    private readonly string _filePath = filePath;
    private T _config = new();

    /// <summary>
    /// Gets the current configuration object
    /// </summary>
    public T Config
    {
        get { return _config; }
    }

    /// <summary>
    /// Loads configuration from the file using streams
    /// </summary>
    /// <returns>The loaded configuration object</returns>
    public T Load()
    {
        try
        {
            if (File.Exists(_filePath))
            {
                using (FileStream fs = new(_filePath, FileMode.Open, FileAccess.Read))
                using (StreamReader sr = new(fs))
                using (JsonTextReader reader = new(sr))
                {
                    JsonSerializer serializer = new();
                    _config = serializer.Deserialize<T>(reader) ?? new T();
                }
                return _config;
            }

            // If the file doesn't exist, return the default configuration
            return _config;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error loading configuration from {_filePath}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Saves the current configuration to the file using streams
    /// </summary>
    public void Save()
    {
        try
        {
            // Create directory if it doesn't exist
            string? directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using FileStream fs = new(_filePath, FileMode.Create, FileAccess.Write);
            using StreamWriter sw = new(fs);
            using JsonTextWriter writer = new(sw);

            // Configure for indented formatting
            writer.Formatting = Formatting.Indented;

            JsonSerializer serializer = new();
            serializer.Serialize(writer, _config);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error saving configuration to {_filePath}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Resets the configuration to default values
    /// </summary>
    /// <returns>The default configuration object</returns>
    public T Reset()
    {
        _config = new T();
        return _config;
    }
}