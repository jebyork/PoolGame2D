using PoolGame.Game.Audio;
using UnityEngine;

namespace PoolGame.Gameplay.Ball
{
    public class BallCollisionAudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip[] audioClips;
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            SoundManagerSO.PlaySound(audioClips, transform.position, 0.2f);
        }
    }
}