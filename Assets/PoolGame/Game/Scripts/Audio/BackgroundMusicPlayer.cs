using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PoolGame.Game.Audio
{
    public class BackgroundMusicPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip[] backgroundMusicClips;
        private AudioSource _audioSource;
        private float _clipLength;
        
        
        
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }
        
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            PlayNewClip();
        }
        
        private void PlayNewClip()
        {
            int randomIndex = Random.Range(0, backgroundMusicClips.Length);
            _audioSource.clip = backgroundMusicClips[randomIndex];
            _audioSource.Play();
            StartCoroutine(WaitToChangeClip());
        }
        
        private IEnumerator WaitToChangeClip()
        {
            _clipLength = _audioSource.clip.length;
            yield return new WaitForSeconds(_clipLength);
            PlayNewClip();
        }
        
    }
}