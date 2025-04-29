using Custom.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Audio
{
    /// <summary>
    /// please only use AudioEmitter as runtime audio player.
    /// AudioEmitter can be preplaced though.
    /// </summary>
    public class AudioEmitter : MonoBehaviour, IPoolable
    {
        public event Action OnEndCallback;
        //todo : clear this dictionary when scene changes
        //todo : dont count if soundSO is set to uncount
        private static readonly Dictionary<int, int> audioDictionary = new Dictionary<int, int>(16);

        private AudioSource audioSource;
        private AudioSO currentAudioSO;
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }
        public static void Dbg(AudioSO audioSO)
        {
            Debug.Log(audioDictionary[audioSO.clip.GetHashCode()]);
        }
        private bool IsAudioPlayable(AudioSO audioSO, bool autoIncrement = false)
        {
            if (!audioSO.enableMaxCount) return true;

            int hash = audioSO.clip.GetHashCode();
            audioDictionary.TryGetValue(hash, out int count);

            bool result = count < audioSO.maxCount;
            if (result && autoIncrement)
                audioDictionary[hash] = ++count;
            return result;
        }
        private void DecreaseDictionaryInstance(AudioSO audioSO)
        {
            if (!audioSO.enableMaxCount) return;

            AudioClip currentClip = audioSO.clip;
            int hash = currentClip.GetHashCode();
            int result = audioDictionary[hash]--;
            Debug.Assert(result >= 0, "there is no way this can happen, right? otherwise contact me. -ojy");
        }
        public void OnPopInitialize()
        {
            OnEndCallback = null;
            currentAudioSO = null;
        }

        public void Play(bool destroyOnEnd = false)
        {
            bool flag = currentAudioSO != null;
            Debug.Assert(flag, "playing audio without initialization");

            if (audioSource.isPlaying)
                StopAudio();

            bool flag2 = IsAudioPlayable(currentAudioSO, true);
            if (!flag2)
            {
                Debug.LogWarning($"AudioInstance Reached Max {currentAudioSO.name}");
                return;
            }

            audioSource.Play();
            StartCoroutine(WaitUntilAudioEnd());
            
            IEnumerator WaitUntilAudioEnd()
            {
                while (audioSource.isPlaying)
                {
                    yield return null;
                }
                OnEndCallback?.Invoke();
                DecreaseDictionaryInstance(currentAudioSO);

                if (destroyOnEnd)
                    KillAudio();
            }
        }
        public void PlayWithInit(AudioSO audioSO, bool destroyOnEnd = false)
        {
            if(audioSource.isPlaying)
                StopAudio();
            Initialize(audioSO);
            Play(destroyOnEnd);
        }
        //public void PlayOneShot()
        //{
        //    PlayOneShot(currentAudioSO);
        //}
        //public void PlayOneShot(AudioSO audioSO)
        //{
        //    bool flag = IsCurrentAudioPlayable(currentAudioSO, true);
        //    if (!flag)
        //    {
        //        Debug.LogWarning($"AudioInstance Reached Max {currentAudioSO.name}");
        //        return;
        //    }
        //    else
        //    {
        //        audioSource.PlayOneShot(audioSO.clip);
        //    }
        //}
        //public void PlayOneShotWithInit(AudioSO audioSO)
        //{
        //    Initialize(audioSO);
        //    PlayOneShot(audioSO);
        //}

        public void StopAudio()
        {
            if (!audioSource.isPlaying) return;

            StopAllCoroutines();

            OnEndCallback?.Invoke();                    //should i call this?
            DecreaseDictionaryInstance(currentAudioSO);

            audioSource.Stop();
        }
        public void KillAudio()
        {
            StopAudio();
            MonoGenericPool<AudioEmitter>.Push(this);   //deactivate gameObject, auto cancel Coroutine.
        }
        public void Initialize(AudioSO audioSO)
        {
            if (audioSource.isPlaying)
            {
                Debug.LogWarning($"n:{name}_initializing while playing audio");
                return;
            }

            currentAudioSO = audioSO;

            //global
            audioSource.clip = audioSO.clip;
            audioSource.outputAudioMixerGroup = audioSO.audioMixerGroup;
            //audioSource3D.loop
            //playoneshot
            audioSource.priority = audioSO.priority;
            audioSource.volume = audioSO.volume;
            audioSource.pitch = audioSO.pitch;
            audioSource.panStereo = audioSO.streoPan;
            audioSource.spatialBlend = audioSO.GetSpatialBlend;
            audioSource.reverbZoneMix = audioSO.reverbZoneMix;

            //3DSOUND SETTINGS
            audioSource.dopplerLevel = audioSO.dopplerLevel;
            audioSource.spread = audioSO.spread;
            audioSource.rolloffMode = audioSO.audioRolloffMode;
            audioSource.minDistance = audioSO.minDistance;
            audioSource.maxDistance = audioSO.maxDistance;
            if (audioSO.audioRolloffMode == AudioRolloffMode.Custom)
                audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, audioSO.curve);
        }
    }
}
