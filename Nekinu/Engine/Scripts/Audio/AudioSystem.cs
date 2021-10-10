using System;
using System.Collections.Generic;
using OpenTK.Audio.OpenAL;

namespace Nekinu
{
    public class AudioSystem
    {
        private static List<AudioSource> sources;
        private static List<AudioClip> audioclips;

        public static void InitAudio()
        {
            sources = new List<AudioSource>();
            audioclips = new List<AudioClip>();

            string source = ALC.GetString(ALDevice.Null, AlcGetString.DefaultDeviceSpecifier);

            ALDevice device = ALC.OpenDevice(source);

            if(device == null)
            {
                Crash_Report.generate_crash_report($"Failed to load default OpenAl audio device! {source}");
            }

            ALC.MakeContextCurrent(ALC.CreateContext(device, new int[0]));

            AL.Listener(ALListener3f.Position, 0, 0, 1.0f);
            AL.Listener(ALListener3f.Velocity, 0, 0, 1.0f);

            AL.DistanceModel(ALDistanceModel.InverseDistanceClamped);
        }

        public static void AddSource(AudioSource source)
        {
            sources.Add(source);
        }

        public static void AddAudioClips(AudioClip clip)
        {
            audioclips.Add(clip);
        }

        public static void CleanUpAudio()
        {
            foreach (AudioSource source in sources)
            {
                source.CleanUp();
            }

            foreach (AudioClip clip in audioclips)
            {
                clip.CleanUp();
            }
        }
    }
}
