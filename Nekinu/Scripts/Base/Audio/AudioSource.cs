using Newtonsoft.Json;
using OpenTK.Audio.OpenAL;

namespace Nekinu
{
    public class AudioSource : Component
    {
        [JsonIgnore]
        private int sourceID;

        public float volume { get;set; }

        [JsonProperty]
        public float pitch { get; set; }

        [JsonProperty]
        public float rollOff { get; set; }
        [JsonProperty]
        public float distance { get; set; }
        [JsonProperty]
        private float maxDistance { get; set; }

        public bool isLooping { get; set; }

        [JsonIgnore]
        public bool isPlaying { get; private set; }

        [JsonIgnore]
        private AudioClip currentClip;

        [JsonIgnore]
        private AudioClip queuedClip;

        public AudioSource()
        {
            volume = 50;
            pitch = 1;
            rollOff = 1;
            distance = 10;
            maxDistance = 20;

            isLooping = false;

            sourceID = AL.GenSource();
            AL.Source(sourceID, ALSourcef.Pitch, pitch);

            AL.Source(sourceID, ALSourcef.MinGain, 0);
            AL.Source(sourceID, ALSourcef.Gain, volume);
            AL.Source(sourceID, ALSourcef.MaxGain, 100);

            AL.Source(sourceID, ALSourceb.Looping, isLooping);

            AL.Source(sourceID, ALSource3f.Position, 0, 0, 0);
            AL.Source(sourceID, ALSource3f.Velocity, 0, 0, 0);

            AL.Source(sourceID, ALSourcef.RolloffFactor, rollOff);
            AL.Source(sourceID, ALSourcef.RolloffFactor, distance);
            AL.Source(sourceID, ALSourcef.MaxDistance, maxDistance);

            AudioSystem.AddSource(this);
        }

        public override void Update()
        {
            base.Update();

            AL.Source(sourceID, ALSource3f.Position, parent.transform.position.x, parent.transform.position.y, parent.transform.position.z); ;

            ALSourceState state = AL.GetSourceState(sourceID);

            isPlaying = state == ALSourceState.Playing ? true : false;
        }

        public void AddClipToQueue(AudioClip clip)
        {
            queuedClip = clip;
        }

        public void Play()
        {
            if (queuedClip != null)
            {
                currentClip = queuedClip;
                queuedClip = null;
            }

            Play(currentClip);
        }

        public void Play(AudioClip clip)
        {
            if (isPlaying)
                Stop();

            currentClip = clip;

            AL.Source(sourceID, ALSourcei.Buffer, currentClip.id);
            AL.SourcePlay(sourceID);
        }

        public void Pause()
        {
            AL.SourcePause(sourceID);
        }

        public void Stop()
        {
            AL.SourceStop(sourceID);
        }

        public override void OnDestroy()
        {
            CleanUp();
        }

        public void CleanUp()
        {
            AL.DeleteSource(sourceID);
        }
    }
}