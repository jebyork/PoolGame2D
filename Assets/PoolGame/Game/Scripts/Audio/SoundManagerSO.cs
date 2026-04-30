using UnityEngine;

namespace PoolGame.Game.Audio
{
    [CreateAssetMenu(fileName = "SoundManager", menuName = "SoundManager")]
    public class SoundManagerSO : ScriptableObject
    {
        private static SoundManagerSO _instance;
        
        public static SoundManagerSO Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = Resources.Load<SoundManagerSO>("SoundManager");
                }
                return _instance;
            }
        }
        
        public AudioSource audioObject;
        
        private static readonly float VolumeChangeMultiplier = 0.15f;
        private static readonly float PitchChangeMultiplier = 0.2f;

        public static void PlaySound(AudioClip clip, Vector3 soundPos, float volume)
        {
            float randomVolume = volume + Random.Range(-VolumeChangeMultiplier, VolumeChangeMultiplier);
            float randomPitch = 1f + Random.Range(-PitchChangeMultiplier, PitchChangeMultiplier);
            AudioSource audioSource = Instantiate(Instance.audioObject, soundPos, Quaternion.identity);
            
            audioSource.clip = clip;
            audioSource.volume = randomVolume;
            audioSource.pitch = randomPitch;
            audioSource.Play();
        }
        public static void PlaySound(AudioClip[] clip, Vector3 soundPos, float volume)
        {
            int randomIndex = Random.Range(0, clip.Length);
            float randomVolume = volume + Random.Range(-VolumeChangeMultiplier, VolumeChangeMultiplier);
            float randomPitch = 1f + Random.Range(-PitchChangeMultiplier, PitchChangeMultiplier);
            AudioSource audioSource = Instantiate(Instance.audioObject, soundPos, Quaternion.identity);
            
            audioSource.clip = clip[randomIndex];
            audioSource.volume = randomVolume;
            audioSource.pitch = randomPitch;
            audioSource.Play();
        }
        
    }
}
