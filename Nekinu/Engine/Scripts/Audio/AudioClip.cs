using System;
using System.IO;
using System.Runtime.InteropServices;
using Nekinu.EngineDebug;
using OpenTK.Audio.OpenAL;

namespace Nekinu
{
    public class AudioClip
    {
        public int id { get; private set; }

        public AudioClip(string clip)
        {
            id = AL.GenBuffer();

            int channels = 0;
            int bits = 0;
            int rate = 0;

            byte[] bytes = new byte[0];

            try
            {
                StreamReader stream = new StreamReader(Directory.GetCurrentDirectory() + clip);

                //https://github.com/mono/opentk/blob/master/Source/Examples/OpenAL/1.1/Playback.cs
                if (stream != null)
                {
                    using (BinaryReader reader = new BinaryReader(stream.BaseStream))
                    {
                        // RIFF header
                        string signature = new string(reader.ReadChars(4));
                        if (signature != "RIFF")
                            throw new NotSupportedException("Specified stream is not a wave file.");

                        int riff_chunck_size = reader.ReadInt32();

                        string format = new string(reader.ReadChars(4));
                        if (format != "WAVE")
                            throw new NotSupportedException("Specified stream is not a wave file.");

                        // WAVE header
                        string format_signature = new string(reader.ReadChars(4));
                        if (format_signature != "fmt ")
                            throw new NotSupportedException("Specified wave file is not supported.");

                        int format_chunk_size = reader.ReadInt32();
                        int audio_format = reader.ReadInt16();
                        int num_channels = reader.ReadInt16();
                        int sample_rate = reader.ReadInt32();
                        int byte_rate = reader.ReadInt32();
                        int block_align = reader.ReadInt16();
                        int bits_per_sample = reader.ReadInt16();

                        string data_signature = new string(reader.ReadChars(4));
                        if (data_signature != "data")
                            throw new NotSupportedException("Specified wave file is not supported.");

                        int data_chunk_size = reader.ReadInt32();

                        channels = num_channels;
                        bits = bits_per_sample;
                        rate = sample_rate;

                        bytes = reader.ReadBytes((int) reader.BaseStream.Length);
                    }
                }

                IntPtr data = Marshal.AllocHGlobal(bytes.Length);
                Marshal.Copy(bytes, 0, data, bytes.Length);

                AL.BufferData(id, GetSoundFormat(channels, bits), data, bytes.Length, rate);
            }
            catch (Exception e)
            {
                Debug.WriteError($"Error loading audio! {e}");
            }
        }

        //https://github.com/mono/opentk/blob/master/Source/Examples/OpenAL/1.1/Playback.cs
        private ALFormat GetSoundFormat(int channels, int bits)
        {
            switch (channels)
            {
                case 1: return bits == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
                case 2: return bits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
                default: throw new NotSupportedException("The specified sound format is not supported.");
            }
        }

        public void CleanUp()
        {
            AL.DeleteBuffer(id);
        }
    }
}