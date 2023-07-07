using UnityEngine;

namespace UI.Helpers
{
    [RequireComponent(typeof(AudioSource))]
    public class SFXPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _source;

        public void Play(AudioClip sound)
        {
            _source.PlayOneShot(sound);
            Destroy(gameObject, sound.length);
        }
    }
}