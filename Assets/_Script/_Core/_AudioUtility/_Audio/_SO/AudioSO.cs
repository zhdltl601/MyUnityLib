using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Custom.Audio
{
    //[Serializable]
    //public struct AudioStruct
    //{
    //    public AudioMixerGroup audioMixerGroup;
    //    public AudioClip clip;
    //    [Range(0, 1)] public float volume;// = 1;
    //    [Range(-3, 3)] public float pitch;// = 0;
    //    public bool is3D;
    //    public bool isLoop;
    //}   

    [CreateAssetMenu(fileName = "AudioSO", menuName = "Scriptable Objects/AudioSO")]
    public class AudioSO : ScriptableObject
    {

        [Header("General")]
        public AudioClip clip;
        public AudioMixerGroup audioMixerGroup;
        public bool loop;
        public bool enableMaxCount = true;
        public int maxCount = 5;

        [Header("Global Values, StartValue")]
        [Space(10)]
        [Range(0, 256)]     public int priority = 128;
        [Range(0, 1)]       public float volume = 1;
        [Range(-3, 3)]      public float pitch = 1;
        [Range(-1, 1)]      public float streoPan = 0;
        [SerializeField]    private bool is3D = true;   // this was float but i changed it to bool. (do we need 2.5D?
        [Range(0, 1.1f)]    public float reverbZoneMix = 1;
        [Range(0, 5)]       public float dopplerLevel = 0;

        [Header("3D Sound Settings")]
        [Range(0, 360)]     public int spread;

        [Range(0, 1000)]    public int minDistance = 1;
        [Range(1.01f, 1000)] public int maxDistance = 500;
        public AudioRolloffMode audioRolloffMode = AudioRolloffMode.Linear;
        [HideInInspector]   public AnimationCurve curve = AnimationCurve.Linear(0, 1, 1, 0);

        public float GetSpatialBlend => is3D ? 1 : 0;
        
        //[SerializeField] AudioStruct audioStruct;
        //public AudioStruct GetAudioStruct => audioStruct;
    }
}
