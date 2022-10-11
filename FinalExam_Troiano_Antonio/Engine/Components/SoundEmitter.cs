using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Audio;

namespace FinalExam_Troiano_Antonio
{
    class SoundEmitter : Component
    {
        AudioSource source;
        AudioClip clip;

        public float Volume { get { return source.Volume; } set { source.Volume = value; } }
        public float Pitch { get { return source.Pitch; } set { source.Pitch = value; } }

        public SoundEmitter(GameObject owner, string clipName) : base(owner)
        {
            source = new AudioSource();
            clip = GfxMgr.GetClip(clipName);
        }

        public void Play(float volume, float pitch = 1f)
        {
            source.Volume = volume;
            source.Pitch = pitch;
            source.Play(clip);
        }
        public void PlayRandom(float volume)
        {
            source.Volume = volume;
            RandomizePitch();
            source.Play(clip);
        }
        protected void RandomizePitch()
        {
            source.Pitch = RandomGenerator.GetRandomFloat() * 0.4f + 0.8f;//0.8 => 1.2
        }
        public void Play(bool loop = false)
        {
            source.Play(clip, loop);
        }


    }
}
