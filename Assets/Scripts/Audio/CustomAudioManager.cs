using UnityEngine.Audio;
using System;
using UnityEngine;

namespace Project.Audio
{
    public class CustomAudioManager : MonoBehaviour
    {
        public Sound[] sounds;

        void Awake()
        {
            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.loop = s.loop;
            }
        }

        public void Play(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
                return;
            s.source.Play();
        }

        public void Stop(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
                return;
            s.source.Stop();
        }
    }
}