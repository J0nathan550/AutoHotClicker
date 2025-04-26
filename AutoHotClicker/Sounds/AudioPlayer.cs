using CSCore;
using System.IO;
using System.Reflection;
using CSCore.Codecs;
using CSCore.SoundOut;

namespace AutoHotClicker.Sounds;

public class AudioPlayer
{
    public static void PlayEmbeddedMp3(string resourceName)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        using Stream resourceStream = assembly.GetManifestResourceStream(resourceName) ?? throw new ArgumentException("Resource not found: " + resourceName);

        // Save the resource stream to a temporary file
        string tempFile = Path.GetTempFileName();
        string tempmp3File = Path.ChangeExtension(tempFile, ".mp3");
        using (FileStream fileStream = File.Create(tempmp3File))
        {
            resourceStream.CopyTo(fileStream);
        }

        // Now use CSCore's normal codec loading from filename
        using (IWaveSource soundSource = CodecFactory.Instance.GetCodec(tempmp3File))
        using (ISoundOut soundOut = GetSoundOut())
        {
            soundOut.Initialize(soundSource);
            soundOut.Play();

            while (soundOut.PlaybackState == PlaybackState.Playing)
            {
                Thread.Sleep(100);
            }
        }

        // Clean up temp file
        File.Delete(tempmp3File);
    }

    private static ISoundOut GetSoundOut()
    {
        if (WasapiOut.IsSupportedOnCurrentPlatform)
            return new WasapiOut();
        else
            return new DirectSoundOut();
    }
}